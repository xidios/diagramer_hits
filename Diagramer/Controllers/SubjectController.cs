using Diagramer.Data;
using Microsoft.AspNetCore.Mvc;

namespace Diagramer.Controllers;

public class SubjectController : Controller
{
    private ApplicationDbContext _context;
    public SubjectController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }
}