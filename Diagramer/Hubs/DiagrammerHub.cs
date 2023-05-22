using System.Collections.Concurrent;
using Diagramer.Constans;
using Diagramer.Data;
using Diagramer.Models;
using Diagramer.Models.Hub;
using Diagramer.Models.Identity;
using Diagramer.Models.mxGraph;
using Diagramer.Models.MxGraphData;
using Diagramer.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Hub = Microsoft.AspNetCore.SignalR.Hub;
using Task = System.Threading.Tasks.Task;

namespace Diagramer.Hubs;

[Authorize]
public class DiagrammerHub : Hub
{
    private ApplicationDbContext _context;

    private readonly IUserService _userService;

    //TODO: изменить
    private static Dictionary<string, Guid> RoomGraphModel = new Dictionary<string, Guid>();

    public DiagrammerHub(ApplicationDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }


    public async Task JoinRoom(string roomId)
    {
        string connectionId = Context.ConnectionId;
        var userId = _userService.GetCurrentUserGuid(Context.User);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        var room = await _context.Rooms
            .Include(r => r.Group)
            .Include(r => r.Task)
            .FirstOrDefaultAsync(r => r.Id == Guid.Parse(roomId));

        if (!RoomGraphModel.ContainsKey(roomId))
        {
            var graph = await _context.MxGraphModels.FirstOrDefaultAsync(g => g.RoomId == room.Id);
            if (graph == null)
            {
                return;
            }

            RoomGraphModel.Add(roomId, graph.MxGraphModelId);
        }

        var hubConnection = new HubConnection
        {
            Id = connectionId,
            //TODO: У задания вставить ID.
            TaskId = room.Task.Id,
            Task = room.Task,
            UserId = userId,
            User = user,
            Group = room.Group,
            GroupId = room.Group.Id,
            Room = room,
            RoomId = room.Id
        };
        await _context.Connections.AddAsync(hubConnection);
        room.HubConnections.Add(hubConnection);
        _context.Update(room);
        await _context.SaveChangesAsync();
        await Groups.AddToGroupAsync(connectionId, room.Id.ToString());
        await Clients.Groups(room.Id.ToString())
            .SendAsync("ReceiveMessage", $"{HTMLHelpers.NameOfUser(user)} has joined the group {room.Id}.");
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        string connectionId = Context.ConnectionId;
        Console.Write(connectionId + " " + "disconnected");
        var userId = _userService.GetCurrentUserGuid(Context.User);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var connection = await _context.Connections.FirstOrDefaultAsync(c => c.Id == connectionId);
        if (connection == null)
        {
            return;
        }

        _context.Connections.Remove(connection);
        await _context.SaveChangesAsync();
        await Clients.All.SendAsync("ReceiveMessage", $"{HTMLHelpers.NameOfUser(user)} has disconnected the group");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    // public async Task<Room> CreateRoom(Models.Task task, Group group)
    // {
    //     Room room = new Room
    //     {
    //         Group = group,
    //         GroupId = group.Id,
    //         Task = task,
    //         TaskId = task.Id
    //     };
    //     await _context.Rooms.AddAsync(room);
    //     await _context.SaveChangesAsync();
    //     return room;
    // }

    public async Task MoveCells(string[] cellIds, int dx, int dy, bool clone)
    {
        //TODO: OtherInGroup
        await Clients.Others.SendAsync("MoveCells", cellIds, dx, dy);
    }

    private MxPoint? CreateNewPointIfExists(MxGeometry newGeometry, MxPointData point, string asPoint)
    {
        if (point != null)
        {
            var newPoint = new MxPoint()
            {
                MxGeometry = newGeometry, MxGeometryId = newGeometry.MxGeometryId, X = point.X,
                Y = point.Y, As = asPoint
            };
            newGeometry.Position.Add(newPoint);
            return newPoint;
        }

        return null;
    }

    private async Task CreateNewCells(List<MxCellData> cells, string roomId)
    {
        var graphId = RoomGraphModel[roomId];
        foreach (var cell in cells)
        {
            var checkCell =
                await _context.MxCells.FirstOrDefaultAsync(c =>
                    c.Id == cell.Id && c.MxGraphModelId == graphId);
            if (checkCell != null)
            {
                continue;
            }

            var newCell = new MxCell()
            {
                MxGraphModelId = graphId,
                Id = cell.Id,
                Style = cell.Style,
                Value = cell.Value ?? "",
                IsEdge = cell.IsEdge ? 1 : 0,
                IsVertex = cell.IsVertex ? 1 : 0,
                ParentId = cell.ParentId,
                SourceId = cell.SourceId,
                TargetId = cell.TargetId,
            };
            var geometry = cell.Geometry;
            var newGeometry = new MxGeometry()
            {
                MxCell = newCell,
                MxCellId = newCell.MxCellId,
                CellId = cell.Id,
                As = "geometry",
                X = geometry.X,
                Y = geometry.Y,
                Height = geometry.Height,
                Width = geometry.Width,
                Relative = geometry.Relative ? 1 : 0,
            };
            newCell.MxGeometry = newGeometry;
            newCell.MxGeometryId = newGeometry.MxGeometryId;
            if (geometry.SourcePoint != null || geometry.TargetPoint != null || geometry.Offset != null)
            {
                newGeometry.Position = new List<MxPoint>();
                CreateNewPointIfExists(newGeometry, geometry.SourcePoint, "sourcePoint");
                CreateNewPointIfExists(newGeometry, geometry.TargetPoint, "targetPoint");
                CreateNewPointIfExists(newGeometry, geometry.Offset, "offset");
            }

            if (geometry.Points != null && geometry.Points.Count > 0)
            {
                newGeometry.Array = new MxArray()
                {
                    MxGeometry = newGeometry,
                    MxGeometryId = newGeometry.MxGeometryId,
                    As = "points"
                };
                foreach (var point in geometry.Points)
                {
                    var newPoint = new MxPoint()
                    {
                        MxArray = newGeometry.Array,
                        MxArrayId = newGeometry.Array.MxArrayId,
                        X = point.X,
                        Y = point.Y
                    };
                    newGeometry.Array.MxPoints.Add(newPoint);
                }
            }

            await _context.MxCells.AddAsync(newCell);
            //_context.Update(graph);
            await _context.SaveChangesAsync();
            if (cell.Children != null && cell.Children.Count > 0) CreateNewCells(cell.Children, roomId);
        }
    }

    public async Task AddCellsOnDiagram(string json, string roomId)
    {
        List<MxCellData> cells = JsonConvert.DeserializeObject<List<MxCellData>>(json);
        await CreateNewCells(cells, roomId);
        await Clients.OthersInGroup(roomId).SendAsync("AddCellsOnDiagram", json);
    }

    public async Task RemoveCells(string json, string roomId)
    {
        await Clients.OthersInGroup(roomId).SendAsync("RemoveCells", json);
    }

    public async Task AddEdgeOnDiagram(string json, string roomId)
    {
        await Clients.OthersInGroup(roomId).SendAsync("AddEdgeOnDiagram", json);
        MxCellData? cell = JsonConvert.DeserializeObject<MxCellData>(json);
        if (cell != null)
        {
            await CreateNewCells(new List<MxCellData> { cell }, roomId);
        }
    }

    public async Task MxGeometryChange(string json, string roomId)
    {
        MxGeometryData geometry = JsonConvert.DeserializeObject<MxGeometryData>(json);
        var graphId = RoomGraphModel[roomId];

        var cell = await _context.MxCells
            .Include(c => c.MxGeometry)
            .ThenInclude(g => g.Array)
            .ThenInclude(a => a.MxPoints)
            .Include(c => c.MxGeometry)
            .ThenInclude(g => g.Position)
            .FirstOrDefaultAsync(c => c.Id == geometry.CellId && c.MxGraphModelId == graphId);
        if (cell != null)
        {
            var cellGeometry = cell.MxGeometry;
            cellGeometry.X = geometry.X;
            cellGeometry.Y = geometry.Y;
            cellGeometry.Height = geometry.Height;
            cellGeometry.Width = geometry.Width;
            cellGeometry.Relative = geometry.Relative ? 1 : 0;
            if (geometry.Points != null)
            {
                if (cellGeometry.Array == null)
                {
                    cellGeometry.Array = new MxArray
                    {
                        As = "points"
                    };
                    await _context.MxArrays.AddAsync(cellGeometry.Array);
                }
                else
                {
                    cellGeometry.Array.MxPoints.Clear();
                    _context.Update(cellGeometry.Array);
                }

                foreach (var point in geometry.Points)
                {
                    var mxPoint = new MxPoint
                    {
                        X = point.X,
                        Y = point.Y,
                        MxArrayId = cellGeometry.Array.MxArrayId,
                        MxArray = cellGeometry.Array
                    };
                    await _context.MxPoints.AddAsync(mxPoint);
                    cellGeometry.Array.MxPoints.Add(mxPoint);
                }
            }


            if (geometry.SourcePoint != null || geometry.TargetPoint != null || geometry.Offset != null)
            {
                if (cellGeometry.Position == null)
                {
                    cellGeometry.Position = new List<MxPoint>();
                }
                else
                {
                    cellGeometry.Position.Clear();
                    _context.Update(cellGeometry);
                }

                var source = CreateNewPointIfExists(cellGeometry, geometry.SourcePoint, "sourcePoint");
                if (source != null) await _context.MxPoints.AddAsync(source);
                var target = CreateNewPointIfExists(cellGeometry, geometry.TargetPoint, "targetPoint");
                if (target != null) await _context.MxPoints.AddAsync(target);
                var offset = CreateNewPointIfExists(cellGeometry, geometry.Offset, "offset");
                if (offset != null) await _context.MxPoints.AddAsync(offset);
            }


            _context.UpdateRange(cellGeometry);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
            }
        }

        await Clients.OthersInGroup(roomId).SendAsync("MxGeometryChange", json);
    }

    public async Task MxTerminalChange(string json, string roomId)
    {
        MxTerminalChangeData terminalChangeData = JsonConvert.DeserializeObject<MxTerminalChangeData>(json);
        var graphId = RoomGraphModel[roomId];
        var cell = await _context.MxCells.FirstOrDefaultAsync(c =>
            c.Id == terminalChangeData.CellId && c.MxGraphModelId == graphId);
        if (cell != null)
        {
            if (terminalChangeData.Source)
            {
                cell.SourceId = terminalChangeData.TerminalId;
            }
            else
            {
                cell.TargetId = terminalChangeData.TerminalId;
            }

            _context.Update(cell);
            await _context.SaveChangesAsync();
        }

        await Clients.OthersInGroup(roomId).SendAsync("MxTerminalChange", json);
    }

    public async Task MxStyleChange(string json, string roomId)
    {
        var styleChangeData = JsonConvert.DeserializeObject<MxStyleChangeData>(json);
        var graphId = RoomGraphModel[roomId];
        var cell = await _context.MxCells.FirstOrDefaultAsync(c =>
            c.Id == styleChangeData.CellId && c.MxGraphModelId == graphId);
        if (cell != null)
        {
            cell.Style = styleChangeData.Style;
            _context.Update(cell);
            await _context.SaveChangesAsync();
        }

        await Clients.OthersInGroup(roomId).SendAsync("MxStyleChange", json);
    }

    public async Task MxValueChange(string json, string roomId)
    {
        var valueChangeData = JsonConvert.DeserializeObject<MxValueChangeData>(json);
        var graphId = RoomGraphModel[roomId];
        var cell = await _context.MxCells.FirstOrDefaultAsync(c =>
            c.Id == valueChangeData.CellId && c.MxGraphModelId == graphId);
        if (cell != null)
        {
            cell.Value = valueChangeData.Value;
            _context.Update(cell);
            await _context.SaveChangesAsync();
        }
        await Clients.OthersInGroup(roomId).SendAsync("MxValueChange", json);
    }

    public async Task MxChildChange(string json, string roomId)
    {
        var childChangeData = JsonConvert.DeserializeObject<MxChildChangeData>(json);
        var graphId = RoomGraphModel[roomId];
        var cell = await _context.MxCells.FirstOrDefaultAsync(c =>
            c.Id == childChangeData.ChildId && c.MxGraphModelId == graphId);
        if (cell != null)
        {
            if (childChangeData.ParentId == null)
            {
                _context.Remove(cell);
            }
            cell.ParentId = childChangeData.ParentId;
            //TODO: cell index
            await _context.SaveChangesAsync();
        }
        await Clients.OthersInGroup(roomId).SendAsync("MxChildChange", json);
    }

    public async Task MxCollapseChange(string json, string roomId)
    {
        //TODO: 
        await Clients.OthersInGroup(roomId).SendAsync("MxCollapseChange", json);
    }
}