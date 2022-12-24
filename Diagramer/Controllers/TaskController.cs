using Diagramer.Data;
using Diagramer.Models;
using Diagramer.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diagramer.Controllers;

[Route("tasks")]
public class TaskController : Controller
{
    private ApplicationDbContext _context;
    public TaskController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Route("")]
    public IActionResult Index()
    {
        return View(_context.Tasks.ToList());
    }

    [HttpGet]
    [Route("create_task")]
    public IActionResult CreateTask()
    {
        return View(new CreateTaskViewModel());
    }
    
    [HttpPost]
    [Route("create_task")]
    public async Task<IActionResult> CreateTask(CreateTaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Diagram? diagram = null;
        if (model.Diagram is not null)
        {
            diagram = new Diagram
            {
                XML = model.Diagram
            };
        }
        
        var task = new Models.Task
        {
            Name = model.Name,
            Description = model.Description,
            Deadline = model.Deadline,
            IsVisible = model.IsVisible,
            Diagram = diagram
        };
        if (diagram is not null)
        {
            await _context.Diagrams.AddAsync(diagram);
        }
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> ViewTask(Guid id)
    {
        var task = await _context.Tasks
            .Include(t=>t.Diagram)
            .FirstOrDefaultAsync(t => t.Id == id);
        return View(task);
    }
}