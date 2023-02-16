using Diagramer.Data;
using Diagramer.Models;
using Diagramer.Models.Enums;
using Diagramer.Models.Identity;
using Diagramer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diagramer.Controllers;

[Route("answer")]
[Authorize]
public class AnswerController : Controller
{
    private ApplicationDbContext _context;
    private readonly IUserService _userService;
    private readonly IDiagrammerService _diagrammerService;

    public AnswerController(ApplicationDbContext context, IUserService userService,
        IDiagrammerService diagrammerService)
    {
        _context = context;
        _userService = userService;
        _diagrammerService = diagrammerService;
    }

    [Route("{answerId:guid}")]
    public async Task<IActionResult> Index(Guid answerId)
    {
        var answer = await _context.Answers
            .Include(a => a.StudentDiagram)
            .Include(a => a.TeacherDiagram)
            .FirstOrDefaultAsync(a => a.Id == answerId);
        if (answer == null)
        {
            return NotFound("Answer not found");
        }

        return View(answer);
    }

    [Route("create/{taskId:guid}")]
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
                    StudentDiagram = new Diagram
                    {
                        XML = _diagrammerService.ReturnTaskDiagramOrEmpty(task.Diagram?.XML)
                    }
                };
                //task.Answers.Add(new_answer);
                await _context.Answers.AddAsync(new_answer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewTask", "Task", new { id = taskId });
        }
        catch (ArgumentNullException)
        {
            return NotFound("User not found");
        }
    }

    [Route("send_to_review/{answerId:guid}")]
    public async Task<IActionResult> SubmitAnswerForReview(Guid answerId)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId);
        if (answer == null)
        {
            return NotFound("Answer not found");
        }

        answer.Status = AnswerStatusEnum.Sent;
        _context.Update(answer);
        await _context.SaveChangesAsync();
        return RedirectToAction("ViewTask", "Task", new { id = answer.TaskId });
    }

    [Route("cancel_answer_submission/{answerId:guid}")]
    public async Task<IActionResult> CancelAnswerSubmission(Guid answerId)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId);
        if (answer == null)
        {
            return NotFound("Answer not found");
        }

        if (answer.Status is AnswerStatusEnum.Rated or AnswerStatusEnum.UnderEvaluation)
        {
            return BadRequest("Answer is not editable");
        }

        answer.Status = AnswerStatusEnum.InProgress;
        _context.Update(answer);
        await _context.SaveChangesAsync();
        return RedirectToAction("ViewTask", "Task", new { id = answer.TaskId });
    }

    [Route("start_review/{answerId:guid}")]
    public async Task<IActionResult> StartAnswerReview(Guid answerId)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId);
        if (answer == null)
        {
            return NotFound("Answer not found");
        }

        answer.Status = AnswerStatusEnum.UnderEvaluation;
        _context.Update(answer);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index",  new { answerId = answer.Id });
    }

    [Route("cancel_review/{answerId:guid}")]
    public async Task<IActionResult> CancelAnswerReview(Guid answerId)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId);
        if (answer == null)
        {
            return NotFound("Answer not found");
        }

        answer.Status = AnswerStatusEnum.Sent;
        _context.Update(answer);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index",  new { answerId = answer.Id });
    }

    [HttpPost]
    [Route("rate/{answerId:guid}")]
    public async Task<IActionResult> RateAnswer(Guid answerId, float Mark, string? Comment)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId);
        if (answer == null)
        {
            return NotFound("Answer not found");
        }

        answer.Mark = Mark;
        answer.Comment = Comment;
        answer.Status = AnswerStatusEnum.Rated;
        _context.Update(answer);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index",  new { answerId = answer.Id });
    }

    [HttpPost]
    [Route("finalize/{answerId:guid}")]
    public async Task<IActionResult> FinalizeAnswer(Guid answerId, string? Comment)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId);
        if (answer == null)
        {
            return NotFound("Answer not found");
        }

        answer.Comment = Comment;
        answer.Status = AnswerStatusEnum.Finalize;
        _context.Update(answer);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index",  new { answerId = answer.Id });
    }

    [Route("make_edits_in_student_answer/{answerId:guid}")]
    public async Task<IActionResult> MakeEditsInStudentAnswer(Guid answerId)
    {
        var answer = await _context.Answers
            .Include(d => d.StudentDiagram)
            .Include(d => d.TeacherDiagram)
            .FirstOrDefaultAsync(a => a.Id == answerId);
        if (answer == null)
        {
            return NotFound("Answer not found");
        }

        if (answer.TeacherDiagram == null)
        {
            answer.TeacherDiagram = new Diagram
            {
                XML = answer.StudentDiagram.XML
            };
            await _context.Diagrams.AddAsync(answer.TeacherDiagram);
            _context.Update(answer);
            await _context.SaveChangesAsync();
        }


        return RedirectToRoute("Diagrammer", new { diagramId = answer.TeacherDiagram.Id, editable = true });
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

        return RedirectToAction("ViewTask", "Task", new { id = taskId });
    }
}