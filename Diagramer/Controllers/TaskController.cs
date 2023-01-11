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

    public TaskController(ApplicationDbContext context, IUserService userService,IDiagrammerService diagrammerService)
    {
        _context = context;
        _userService = userService;
        _diagrammerService = diagrammerService;
    }

    [Route("")]
    public IActionResult Index()
    {
        return View(_context.Tasks.ToList());
    }

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
            Subject_id = subject_id,
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

        List<Category> categories = await _context.Categories.Where(c => model.CategoriesIds.Any(i => i == c.Id)).ToListAsync();
        var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == model.Subject_id);
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
            Categories = categories
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
    [Route("{id:guid}")]
    public async Task<IActionResult> ViewTask(Guid id)
    {
        try
        {
            var userIdGuid = _userService.GetCurrentUserGuid(User);
            var task = await _context.Tasks
                .Include(t => t.Diagram)
                .FirstOrDefaultAsync(t => t.Id == id);
            var answer = await _context.Answers
                .Include(a=>a.Diagram)
                .FirstOrDefaultAsync(a => a.TaskId == task.Id && a.UserId == userIdGuid);
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

    //[HttpPost]
    [Route("create_answer/{taskId:guid}")]
    public async Task<IActionResult> CreateAnswer(Guid taskId)
    {
        try
        {
            var userIdGuid = _userService.GetCurrentUserGuid(User);
            var task = await _context.Tasks
                .Include(t => t.Diagram)
                .FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
            {
                return NotFound("Task not found");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userIdGuid);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var answer = await _context.Answers.FirstOrDefaultAsync(a => a.TaskId == taskId && a.UserId == userIdGuid);
            if (answer == null)
            {
                var new_answer = new Answer
                {
                    Task = task,
                    TaskId = taskId,
                    User = user,
                    UserId = userIdGuid,
                    Diagram = new Diagram
                    {
                        XML = _diagrammerService.ReturnTaskDiagramOrEmpty(task.Diagram?.XML)
                    }
                };
                task.Answers.Append(new_answer);
                await _context.Answers.AddAsync(new_answer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewTask", new {id = taskId});
        }
        catch (ArgumentNullException)
        {
            return NotFound("User not found");
        }
    }

    [Route("send_to_review/{answerId:guid}")]
    public async Task<IActionResult> SendAnswerToReview(Guid answerId)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId);
        if (answer == null)
        {
            return NotFound("Answer not found");
        }

        answer.Status = AnswerStatusEnum.Sent;
        _context.Update(answer);
        await _context.SaveChangesAsync();
        return RedirectToAction("ViewTask", new { id = answer.TaskId });
    }

    public async Task<IActionResult> DeleteAnswer(Guid answerId)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId);
        if (answer == null)
        {
            return NotFound("Answer not found");
        }

        var taskId = answer.TaskId;
        _context.Remove(answer);
        await _context.SaveChangesAsync();
        
        return RedirectToAction("ViewTask", new {id = taskId});
    }
}