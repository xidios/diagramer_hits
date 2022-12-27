using Diagramer.Data;
using Diagramer.Models;
using Diagramer.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diagramer.Controllers;

[Route("subject")]
[Authorize]
public class SubjectController : Controller
{
    private ApplicationDbContext _context;

    public SubjectController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Route("all")]
    public async Task<IActionResult> Index()
    {
        var subjects = await _context.Subjects.ToListAsync();
        return View(subjects);
    }

    [Route("{id:guid}")]
    public async Task<IActionResult> OpenSubject(Guid id)
    {
        var subject = await _context.Subjects
            .Include(s=>s.Tasks)
            .FirstOrDefaultAsync(x => x.Id == id);
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
            ModelState.AddModelError("Name","Предмет уже существует");
            return View(model);
        }

        await _context.Subjects.AddAsync(new Subject
        {
            Name = model.Name
        });
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}