using System.Globalization;
using Diagramer.Data;
using Diagramer.Models;
using Diagramer.Models.Enums;
using Diagramer.Models.Identity;
using Diagramer.Models.ViewModels;
using Diagramer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Diagramer.Controllers;

[Route("tasks")]
[Authorize]
public class TaskController : Controller
{
    private ApplicationDbContext _context;
    private readonly IUserService _userService;
    private readonly IDiagrammerService _diagrammerService;

    public TaskController(ApplicationDbContext context, IUserService userService, IDiagrammerService diagrammerService,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userService = userService;
        _diagrammerService = diagrammerService;
    }

    // [Route("")]
    // public IActionResult Index()
    // {
    //     return View(_context.Tasks.ToList());
    // }

    [HttpGet]
    [Route("create_task", Name = "CreateTask")]
    public async Task<IActionResult> CreateTask(Guid subject_id)
    {
        if (subject_id == null)
        {
            return NotFound();
        }

        return View(new CreateTaskViewModel
        {
            SubjectId = subject_id,
            Categories = _context.Categories.ToList()
        });
    }

    [HttpPost]
    [Route("create_task")]
    public async Task<IActionResult> CreateTask(CreateTaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        List<Category> categories =
            await _context.Categories.Where(c => model.CategoriesIds.Any(i => i == c.Id)).ToListAsync();
        var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == model.SubjectId);
        if (subject == null)
        {
            return NotFound("Subject not found");
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
            Diagram = diagram,
            Subject = subject,
            Categories = categories,
            IsGroupTask = model.IsGroupTask
        };
        subject.Tasks.Add(task);
        if (diagram is not null)
        {
            await _context.Diagrams.AddAsync(diagram);
        }

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        return RedirectToAction("OpenSubject", "Subject", new { id = subject.Id });
    }

    [HttpGet]
    [Route("edit_task")]
    public async Task<IActionResult> EditTask(Guid task_id)
    {
        var task = await _context.Tasks
            .Include(t=>t.Diagram)
            .Include(t=>t.Categories)
            .FirstOrDefaultAsync(t => t.Id == task_id);

        return View(new EditTaskViewModel
        {
            TaskId = task_id,
            Diagram = task.Diagram?.XML,
            SelectedCategories = task.Categories,
            Categories = await _context.Categories.ToListAsync(),
            Description = task.Description,
            Deadline = task.Deadline,
            IsVisible = task.IsVisible,
            Name = task.Name,
            Mark = task.Mark,
            MarkDescription = task.MarkDescription
        });
    }

    [HttpPost]
    [Route("edit_task")]
    public async Task<IActionResult> EditTask(EditTaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var task = await _context.Tasks
            .Include(t=>t.Diagram)
            .Include(c=>c.Categories)
            .FirstOrDefaultAsync(t => t.Id == model.TaskId);
        if (task == null)
        {
            return NotFound("Task not found");
        }

        List<Category> categories =
            await _context.Categories.Where(c => model.CategoriesIds.Any(i => i == c.Id)).ToListAsync();
        // var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == model.Subject_id);
        // if (subject == null)
        // {
        //     return NotFound("Subject not found");
        // }

        Diagram? diagram = null;
        if (model.Diagram is not null)
        {
            diagram = new Diagram
            {
                XML = model.Diagram
            };
        }


        task.Name = model.Name;
        task.Description = model.Description;
        task.Deadline = model.Deadline;
        task.IsVisible = model.IsVisible;
        task.Diagram = diagram;
        if (model.Diagram is not null)
        {
            if (task.Diagram is null)
            {
                task.Diagram = new Diagram
                {
                    XML = model.Diagram
                };
            }
            else
            {
                task.Diagram.XML = model.Diagram;
            }
            
        }
        // Subject = subject,
        task.Categories = categories;
        task.Mark = model.Mark;
        task.MarkDescription = model.MarkDescription;
        // subject.Tasks.Add(task);
        if (diagram is not null)
        {
            await _context.Diagrams.AddAsync(diagram);
        }

        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return RedirectToAction("ViewTask", "Task", new { id = task.Id });
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> ViewTask(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetCurrentUserGuid(User));
        try
        {
            var task = await _context.Tasks
                .Include(t => t.Diagram)
                .FirstOrDefaultAsync(t => t.Id == id);
            task.Groups.Any(g => user.Groups.Any(u => u.Id == g.Id));
            var answer = await _context.Answers
                .Include(a => a.StudentDiagram)
                .Include(d => d.TeacherDiagram)
                .FirstOrDefaultAsync(a => a.TaskId == task.Id && a.UserId == user.Id);
            var model = new ViewTaskViewModel
            {
                Task = task,
                UserAnswer = answer
            };
            return View(model);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [Route("change_visibility/{task_id:guid}")]
    public async Task<IActionResult> ChangeTaskVisibility(Guid task_id)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == task_id);
        if (task == null)
        {
            return NotFound("Task not found");
        }

        task.IsVisible = !task.IsVisible;
        _context.Update(task);
        await _context.SaveChangesAsync();
        return RedirectToAction("ViewTask", new { id = task_id });
    }

    [HttpGet]
    [Route("{task_id:guid}/answers")]
    public async Task<IActionResult> ViewAnswersOnTask(Guid task_id)
    {
        var answers = await _context.Answers.Where(a => a.TaskId == task_id)
            .Include(a => a.User)
            .ToListAsync();
        return View(answers);
    }
    [Route("{task_id:guid}/groups")]
    public async Task<IActionResult> TaskGroups(Guid task_id)
    {
        var task = await _context.Tasks
            .Include(t=>t.Groups)
            .FirstOrDefaultAsync(t => t.Id == task_id);
        if (task == null)
        {
            return NotFound();
        }
        
        return View(task);
    
        }
    [HttpGet]
    [Route("{task_id:guid}/add_groups")]
    public async Task<IActionResult> AddGroupsToTask(Guid task_id)
    {
        var task = await _context.Tasks
            .Include(t=>t.Groups)
            .FirstOrDefaultAsync(t => t.Id == task_id);
        if (task == null)
        {
            return NotFound();
        }

        List<Group> notSelectedGroups = await _context.Groups
            .Where(group => !task.Groups.Contains(group))
            .ToListAsync();
        return View(new AddGroupsToTaskViewModel
        {
            TaskId = task_id,
            Groups = notSelectedGroups
        });
    }
    [HttpPost]
    [Route("{task_id:guid}/add_groups")]
    public async Task<IActionResult> AddGroupsToTask(AddGroupsToTaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var task = await _context.Tasks
            .Include(t=>t.Groups)
            .FirstOrDefaultAsync(t => t.Id == model.TaskId);
        if (task == null)
        {
            return NotFound();
        }
        
        List<Group> groups =
            await _context.Groups.Where(c => model.SelectedGroupsId.Any(i => i == c.Id)).ToListAsync();
        task.Groups.AddRange(groups);
        _context.Update(task);
        await _context.SaveChangesAsync();
        return RedirectToAction("TaskGroups", new {task_id = model.TaskId});
    }

    [Route("{task_id:guid}/remove_group/{group_id:guid}")]
    public async Task<IActionResult> RemoveGroupFromTask(Guid task_id, Guid group_id)
    {
        var task = await _context.Tasks
            .Include(t=>t.Groups)
            .FirstOrDefaultAsync(t => t.Id == task_id);
        if (task == null)
        {
            return NotFound("Task not found");
        }

        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == group_id);
        if (group == null)
        {
            return NotFound("Group not found");
        }

        task.Groups.Remove(group);
        _context.Update(task);
        await _context.SaveChangesAsync();
        return RedirectToAction("TaskGroups", new { task_id = task_id });
    }
    
}