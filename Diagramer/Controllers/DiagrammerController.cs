using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml.Serialization;
using Diagramer.Data;
using Diagramer.Hubs;
using Diagramer.Models;
using Diagramer.Models.Hub;
using Diagramer.Models.mxGraph;
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
    private IDiagrammerService _diagrammerService;

    public DiagrammerController(ApplicationDbContext context, IDiagrammerService diagrammerService)
    {
        _context = context;
        _diagrammerService = diagrammerService;
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
            ViewBag.IsGroupTask = true;
            var room = await _context.Rooms
                .Include(r=>r.MxGraphModel)
                .ThenInclude(m=>m.Cells)
                .ThenInclude(c=>c.MxGeometry)
                .ThenInclude(g=>g.Position)
                .Include(r=>r.MxGraphModel)
                .ThenInclude(m=>m.Cells)
                .ThenInclude(c=>c.MxGeometry)
                .ThenInclude(g=>g.Array)
                .ThenInclude(a=>a.MxPoints)
                .FirstOrDefaultAsync(r => r.GroupId == groupId && r.TaskId == taskId);
            if (room == null)
            {
                var graph = await _diagrammerService.CreateMxGraphModelByXML(diagram.XML);
                if (graph == null)
                {
                    return BadRequest("Проблемы при создании модели графа");
                }
                room = new Room
                {
                    Group = group,
                    GroupId = group.Id,
                    Task = task,
                    TaskId = task.Id,
                    MxGraphModel = graph,
                    MxGraphModelId = graph.MxGraphModelId
                };
                graph.Room = room;
                graph.RoomId = room.Id;
                await _context.Rooms.AddAsync(room);
                await _context.SaveChangesAsync();
            }
            ViewBag.RoomId = room.Id.ToString();
            ViewBag.Diagram = _diagrammerService.SerializeMxGraphModelToXML(room.MxGraphModel);
            return View(diagram);
        }

        ViewBag.Diagram = diagram.XML;
        return View(diagram);
    }
}