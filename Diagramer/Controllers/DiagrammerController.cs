using System.Reflection.PortableExecutable;
using Diagramer.Data;
using Diagramer.Models;
using Diagramer.Models.Hub;
using Diagramer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diagramer.Controllers;

[Route("diagrammer")]
[Authorize]
public class DiagrammerController : Controller
{
    private ApplicationDbContext _context;

    public DiagrammerController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Route("{diagramId:guid}", Name = "Diagrammer")]
    public async Task<IActionResult> Index(Guid diagramId, Guid? taskId = null, Guid? groupId = null,
        bool editable = true)
    {
        var diagram = _context.Diagrams.FirstOrDefault(d => d.Id == diagramId);
        if (diagram == null)
        {
            return NotFound();
        }

        Models.Task? task = null;
        if (taskId != null)
        {
            task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        Group group = null;
        if (groupId != null)
        {
            group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
        }

        //TODO: Логика проверки юзера в группе не помешала бы
        ViewBag.IsGroupTask = false;
        if (taskId != null && task.IsGroupTask)
        {
            //ViewBag.TaskId = taskId.ToString();
            ViewBag.IsGroupTask = true;
            //ViewBag.GroupId = groupId.ToString();
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.GroupId == groupId && r.TaskId == taskId);
            if (room == null)
            {
                room = new Room
                {
                    Group = group,
                    GroupId = group.Id,
                    Task = task,
                    TaskId = task.Id
                };
                await _context.Rooms.AddAsync(room);
                await _context.SaveChangesAsync();
            }
            ViewBag.RoomId = room.Id.ToString();
        }

        ViewBag.Diagram = diagram.XML;
        return View(diagram);
    }
}