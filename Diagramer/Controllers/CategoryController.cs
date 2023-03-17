using Diagramer.Data;
using Diagramer.Models;
using Diagramer.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diagramer.Controllers;

[Route("category")]
[Authorize]
public class CategoryController : Controller
{
    private ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.ToListAsync();
        return View(categories);
    }

    [HttpGet]
    [Route("create_subject")]
    public async Task<IActionResult> CreateCategory()
    {
        var model = new CreateCategoryViewModel();
        return View(model);
    }

    [HttpPost]
    [Route("create_subject")]
    public async Task<IActionResult> CreateCategory(CreateCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var category = _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == model.Name.ToLower());
        if (category == null)
        {
            return View(model);
        }

        var newCategory = new Category()
        {
            Name = model.Name
        };

        await _context.Categories.AddAsync(newCategory);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [Route("open/{id:guid}")]
    public async Task<IActionResult> OpenCategory(Guid id)
    {
        var category = await _context.Categories
            .Include(c => c.Tasks)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            return NotFound("Category not found");
        }

        return View(category);
    }
}