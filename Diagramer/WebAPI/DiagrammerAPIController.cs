using Diagramer.Data;
using Diagramer.Models.Enums;
using Diagramer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diagramer.WebAPI;

[Route("diagrammer/api")]
public class DiagrammerAPIController : ControllerBase
{
    private ApplicationDbContext _context;
    private IDiagrammerService _diagrammerService;

    public DiagrammerAPIController(ApplicationDbContext context, IDiagrammerService diagrammerService)
    {
        _context = context;
        _diagrammerService = diagrammerService;
    }

    [Route("{diagramId:guid}")]
    [HttpPost]
    public async Task<ActionResult<String>> UpdateTask(Guid diagramId, [FromBody] string diagramXML)
    {
        int code = await _diagrammerService.SaveDiagramToDatabase(diagramId, diagramXML, User);
        switch (code)
        {
            case 404:
                return StatusCode(StatusCodes.Status404NotFound, "Not found");
            case 405:
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "Answer not editable");
            case 403:
                return StatusCode(StatusCodes.Status403Forbidden, "Not enough permissions ");
            case 200:
                return StatusCode(StatusCodes.Status200OK, "Diagram updated");
            default:
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
        }
        
        // var diagram = await _context.Diagrams.FirstOrDefaultAsync(d => d.Id == diagramId);
        // if (diagram == null)
        // {
        //     return StatusCode(StatusCodes.Status404NotFound, "Diagram not found");
        // }
        //
        // var answer = await _context.Answers
        //     .Include(a => a.StudentDiagram)
        //     .FirstOrDefaultAsync(a => a.StudentDiagram.Id == diagramId || a.TeacherDiagram.Id == diagramId);
        // if (answer == null)
        // {
        //     return StatusCode(StatusCodes.Status404NotFound, "Answer not found");
        // }
        //
        // if (answer.TeacherDiagram?.Id == diagramId)
        // {
        //     if (answer.Status != AnswerStatusEnum.UnderEvaluation)
        //     {
        //         return StatusCode(StatusCodes.Status405MethodNotAllowed, "Answer not editable");
        //     }
        //
        //     if (User.IsInRole("Teacher") || User.IsInRole("Admin"))
        //     {
        //         diagram.XML = diagramXML;
        //         _context.Update(diagram);
        //         await _context.SaveChangesAsync();
        //         return "Diagram updated";
        //     }
        //     else
        //     {
        //         return StatusCode(StatusCodes.Status403Forbidden, "Not enough permissions ");
        //     }
        // }
        //
        // if (answer.Status is AnswerStatusEnum.Rated or AnswerStatusEnum.UnderEvaluation or AnswerStatusEnum.Sent)
        // {
        //     return StatusCode(StatusCodes.Status405MethodNotAllowed, "Answer not editable");
        // }
        //
        // //TODO: валидация диаграммы?
        // diagram.XML = diagramXML;
        // _context.Update(diagram);
        // await _context.SaveChangesAsync();
        // return "Diagram updated";
    }
}