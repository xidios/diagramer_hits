using System.Collections.Concurrent;
using Diagramer.Constans;
using Diagramer.Data;
using Diagramer.Models;
using Diagramer.Models.Hub;
using Diagramer.Models.Identity;
using Diagramer.Models.mxGraph;
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
            .Include(r=>r.Group)
            .Include(r=>r.Task)
            .FirstOrDefaultAsync(r => r.Id == Guid.Parse(roomId));

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

    public async Task MoveCells(string[] cellIds, int dx, int dy,bool clone)
    {
        //TODO: OtherInGroup
        await Clients.Others.SendAsync("MoveCells", cellIds, dx, dy);
    }
    

    public async Task AddCellsOnDiagram(string json)
    {
        List<Cell> cells = JsonConvert.DeserializeObject<List<Cell>>(json); 
        await Clients.Others.SendAsync("AddCellsOnDiagram",  json);
    }
    public async Task RemoveCells(string json)
    {
        await Clients.Others.SendAsync("RemoveCells",  json);
    }
    
    public async Task AddEdgeOnDiagram(string json)
    {
        await Clients.Others.SendAsync("AddEdgeOnDiagram",  json);
    }
    public async Task MxGeometryChange(string json)
    {
        //TODO: OtherInGroup
        Geometry geometry = JsonConvert.DeserializeObject<Geometry>(json);
        await Clients.Others.SendAsync("MxGeometryChange", json);
    }
    
    public async Task MxTerminalChange(string json)
    {
        //TODO: OtherInGroup
        await Clients.Others.SendAsync("MxTerminalChange", json);
    }
    public async Task MxStyleChange(string json)
    {
        //TODO: OtherInGroup
        await Clients.Others.SendAsync("MxStyleChange", json);
    }
    public async Task MxValueChange(string json)
    {
        //TODO: OtherInGroup
        await Clients.Others.SendAsync("MxValueChange", json);
    }
    
    public async Task MxChildChange(string json)
    {
        //TODO: OtherInGroup
        await Clients.Others.SendAsync("MxChildChange", json);
    }
    public async Task MxCollapseChange(string json)
    {
        //TODO: OtherInGroup
        await Clients.Others.SendAsync("MxCollapseChange", json);
    }
    
}