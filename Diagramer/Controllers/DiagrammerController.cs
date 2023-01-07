using System.Reflection.PortableExecutable;
using Diagramer.Data;
using Diagramer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult Index(Guid diagramId, bool editable = true)
    {
        var diagram = _context.Diagrams.FirstOrDefault(d => d.Id == diagramId);
        if (diagram == null)
        {
            return NotFound();
        }

        ViewBag.Diagram = diagram.XML;
        return View(diagram);
    }
}