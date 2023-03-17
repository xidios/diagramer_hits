using System.Reflection.PortableExecutable;
using Diagramer.Data;
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

        //TODO: Логика проверки юзера в группе не помешала бы
        ViewBag.IsGroupTask = false;
        if (taskId != null && task.IsGroupTask)
        {
            ViewBag.TaskId = taskId.ToString();
            ViewBag.IsGroupTask = true;
            ViewBag.GroupId = groupId.ToString();
        }

        ViewBag.Diagram = diagram.XML;
        return View(diagram);
    }
}