using System.Collections.Concurrent;
using Diagramer.Constans;
using Diagramer.Data;
using Diagramer.Models;
using Diagramer.Models.Hub;
using Diagramer.Models.Identity;
using Diagramer.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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

    
    public async Task JoinRoom(string taskId,string groupId)
    {
        
        string connectionId = Context.ConnectionId;
        var userId = _userService.GetCurrentUserGuid(Context.User);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var task = await _context.Tasks.FirstOrDefaultAsync(t=>t.Id == Guid.Parse(taskId));
        if (task == null)
        {
            return;
        }
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == Guid.Parse(groupId));
        if (group == null)
        {
            return;
        }
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.GroupId == group.Id && r.TaskId == task.Id);
        if (room == null)
        {
            room = await CreateRoom(task, group);
        }

        var hubConnection = new HubConnection
        {
            Id = connectionId,
            //TODO: У задания вставить ID.
            TaskId = task.Id,
            Task = task,
            UserId = userId,
            User = user,
            Group = group,
            GroupId = group.Id,
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
    
    public async Task<Room> CreateRoom(Models.Task task, Group group)
    {
        Room room = new Room
        {
            Group = group,
            GroupId = group.Id,
            Task = task,
            TaskId = task.Id
        };
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
        return room;
    }
}