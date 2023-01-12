using Diagramer.Data;
using Diagramer.Models.Enums;
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
            return StatusCode(StatusCodes.Status404NotFound,"Diagram not found");
        }

        var answer = await _context.Answers
            .Include(a=>a.Diagram)
            .FirstOrDefaultAsync(a => a.Diagram.Id == diagramId);
        if (answer == null)
        {
            return  StatusCode(StatusCodes.Status404NotFound,"Answer not found");
        }

        if (answer.Status is AnswerStatusEnum.Rated or AnswerStatusEnum.UnderEvaluation or AnswerStatusEnum.Sent)
        {
            return StatusCode(StatusCodes.Status405MethodNotAllowed, "Answer not editable");
        }
        //TODO: валидация диаграммы?
        diagram.XML = diagramXML;
        _context.Update(diagram);
        await _context.SaveChangesAsync();
        return "Diagram updated";
    }
}