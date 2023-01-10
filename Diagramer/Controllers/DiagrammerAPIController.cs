using Diagramer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diagramer.Controllers;

[Route("diagrammer/api")]
public class DiagrammerAPIController : ControllerBase
{
    private ApplicationDbContext _context;
    public DiagrammerAPIController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Route("{diagramId:guid}")]
    [HttpPost]
    public async Task<ActionResult<String>> UpdateTask(Guid diagramId,[FromBody]string diagramXML)
    {
        var diagram = await _context.Diagrams.FirstOrDefaultAsync(d => d.Id == diagramId);
        if (diagram == null)
        {
            return "Diagram not found";
        }
        //TODO: валидация диаграммы?
        diagram.XML = diagramXML;
        _context.Update(diagram);
        await _context.SaveChangesAsync();
        return "Diagram updated";
    }
}