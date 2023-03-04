using Diagramer.Data;
using Diagramer.Models;
using Diagramer.Models.Identity;
using Diagramer.Models.ViewModels;
using Diagramer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diagramer.Controllers;

[Route("subject")]
[Authorize]
public class SubjectController : Controller
{
    private ApplicationDbContext _context;
    private readonly IUserService _userService;

    public SubjectController(ApplicationDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [Route("all")]
    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Admin") || User.IsInRole("Teacher"))
        {
            var subjects = await _context.Subjects.ToListAsync();
            return View(subjects);
        }

        var user = await _context.Users
            .Include(u => u.Subjects)
            .FirstOrDefaultAsync(u => u.Id == _userService.GetCurrentUserGuid(User));
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }

        return View(user.Subjects);
    }

    [Route("{id:guid}")]
    public async Task<IActionResult> OpenSubject(Guid id)
    {
        var subject = await _context.Subjects
            .Include(s => s.Tasks)
            .FirstOrDefaultAsync(x => x.Id == id);
        var user = await _context.Users
            .Include(u => u.Subjects)
            .FirstOrDefaultAsync(u => u.Id == _userService.GetCurrentUserGuid(User));
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }

        if (!user.Subjects.Contains(subject))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "Пользователь не добавлен на данную дисциплину");
        }

        if (subject == null)
        {
            return NotFound();
        }

        return View(subject);
    }

    [HttpGet]
    [Route("create_subject")]
    public async Task<IActionResult> AddSubject()
    {
        var model = new CreateSubjectViewModel();
        return View(model);
    }

    [HttpPost]
    [Route("create_subject")]
    public async Task<IActionResult> AddSubject(CreateSubjectViewModel model)
    {
        var subject = await _context.Subjects.FirstOrDefaultAsync(x => x.Name.ToLower() == model.Name.ToLower());
        if (subject != null)
        {
            ModelState.AddModelError("Name", "Предмет уже существует");
            return View(model);
        }

        await _context.Subjects.AddAsync(new Subject
        {
            Name = model.Name
        });
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("{subject_id:guid}/users")]
    public async Task<IActionResult> GetSubjectUsers(Guid subject_id)
    {
        var subject = await _context.Subjects
            .AsNoTracking()
            .Include(s => s.Users)
            .SingleOrDefaultAsync(s => s.Id == subject_id);
        if (subject == null)
        {
            return NotFound("Предмет не найден");
        }

        return View(subject);
    }

    [HttpGet]
    [Route("{subject_id:guid}/add_users")]
    public async Task<IActionResult> AddUsersToSubject(Guid subject_id)
    {
        var subject = await _context.Subjects
            .Include(s => s.Users)
            .FirstOrDefaultAsync(s => s.Id == subject_id);
        if (subject == null)
        {
            return NotFound();
        }

        List<ApplicationUser> notSelectedUsers = await _context.Users
            .Where(user => !subject.Users.Contains(user))
            .ToListAsync();
        return View(new AddUsersToSubjectViewModel()
        {
            SubjectId = subject_id,
            Users = notSelectedUsers
        });
    }

    [HttpPost]
    [Route("{subject_id:guid}/add_users")]
    public async Task<IActionResult> AddUsersToSubject(AddUsersToSubjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var subject = await _context.Subjects
            .Include(s => s.Users)
            .FirstOrDefaultAsync(s => s.Id == model.SubjectId);
        if (subject == null)
        {
            return NotFound();
        }

        List<ApplicationUser> users =
            await _context.Users.Where(c => model.SelectedUsersId.Any(i => i == c.Id)).ToListAsync();
        subject.Users.AddRange(users);
        _context.Update(subject);
        await _context.SaveChangesAsync();
        return RedirectToAction("GetSubjectUsers", new { subject_id = model.SubjectId });
    }

    [Route("{subject_id:guid}/remove_user/{user_id:guid}")]
    public async Task<IActionResult> RemoveUserFromSubject(Guid subject_id, Guid user_id)
    {
        var subject = await _context.Subjects
            .Include(s => s.Users)
            .FirstOrDefaultAsync(s => s.Id == subject_id);
        if (subject == null)
        {
            return NotFound("Предмет не найден");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == user_id);
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }

        subject.Users.Remove(user);
        _context.Update(subject);
        await _context.SaveChangesAsync();
        return RedirectToAction("GetSubjectUsers", new { subject_id = subject_id });
    }
}