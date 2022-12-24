using System.Reflection.PortableExecutable;
using Diagramer.Data;
using Microsoft.AspNetCore.Mvc;

namespace Diagramer.Controllers;

[Route("diagrammer")]
public class DiagrammerController : Controller
{
    
    private ApplicationDbContext _context;
    public DiagrammerController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Route("{id:guid}", Name="Diagrammer")]
    public IActionResult Index(Guid id, bool editable=true)
    {
        var diagram = _context.Diagrams.FirstOrDefault(d => d.Id == id);
        if (diagram == null)
        {
            return NotFound();
        }
        ViewBag.Diagram = diagram.XML;
        return View(diagram);
    }

}