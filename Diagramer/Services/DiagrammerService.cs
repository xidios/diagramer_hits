using System.Security.Claims;
using System.Text;
using System.Xml.Serialization;
using Diagramer.Constans;
using Diagramer.Data;
using Diagramer.Models.Enums;
using Diagramer.Models.mxGraph;
using Microsoft.EntityFrameworkCore;

namespace Diagramer.Services;

public interface IDiagrammerService
{
    public string ReturnTaskDiagramOrEmpty(string diagramXML);
    public Task<MxGraphModel> CreateMxGraphModelByXML(string? diagramXML);
    public string SerializeMxGraphModelToXML(MxGraphModel graph);
    public Task<int> SaveDiagramToDatabase(Guid diagramId, string diagramXML, ClaimsPrincipal User);
}

public class DiagrammerService : IDiagrammerService
{
    private ApplicationDbContext _context;
    public DiagrammerService(ApplicationDbContext context)
    {
        _context = context;
    }
    public string ReturnTaskDiagramOrEmpty(string? diagramXML)
    {
        return diagramXML == null ? DiagramConstans.EMPTY_DIAGRAM_XML : diagramXML;
    }

    public async Task<MxGraphModel> CreateMxGraphModelByXML(string? diagramXML)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(MxGraphModel));
        MxGraphModel? graph;
        using (StringReader reader = new StringReader(diagramXML))
        {
            graph = (MxGraphModel)serializer.Deserialize(reader);
            foreach (var cell in graph.Cells)
            {
                if (cell.MxGeometry != null)
                {
                    cell.MxGeometry.MxCell = cell;
                    cell.MxGeometry.MxCellId = cell.MxCellId;
                }
            }
            await _context.MxGraphModels.AddAsync(graph);
            await _context.SaveChangesAsync();
        }

        return graph;
    }

    public string SerializeMxGraphModelToXML(MxGraphModel graph)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(MxGraphModel));
        using (var memoryStream = new MemoryStream())
        {
            serializer.Serialize(memoryStream, graph);
        
            // Переводим MemoryStream в строку
            var xmlString = Encoding.UTF8.GetString(memoryStream.ToArray());
            return xmlString;
        }
    }

    public async Task<int> SaveDiagramToDatabase(Guid diagramId, string diagramXML, ClaimsPrincipal User)
    {
        var diagram = await _context.Diagrams.FirstOrDefaultAsync(d => d.Id == diagramId);
        if (diagram == null)
        {
            return StatusCodes.Status404NotFound;
        }

        var answer = await _context.Answers
            .Include(a => a.StudentDiagram)
            .FirstOrDefaultAsync(a => a.StudentDiagram.Id == diagramId || a.TeacherDiagram.Id == diagramId);
        if (answer == null)
        {
            return StatusCodes.Status404NotFound;
        }

        if (answer.TeacherDiagram?.Id == diagramId)
        {
            if (answer.Status != AnswerStatusEnum.UnderEvaluation)
            {
                return StatusCodes.Status403Forbidden;
            }

            if (User.IsInRole("Teacher") || User.IsInRole("Admin"))
            {
                diagram.XML = diagramXML;
                _context.Update(diagram);
                await _context.SaveChangesAsync();
                return StatusCodes.Status200OK;
            }
            else
            {
                return StatusCodes.Status403Forbidden;
            }
        }

        if (answer.Status is AnswerStatusEnum.Rated or AnswerStatusEnum.UnderEvaluation or AnswerStatusEnum.Sent)
        {
            return StatusCodes.Status403Forbidden;
        }

        //TODO: валидация диаграммы?
        diagram.XML = diagramXML;
        _context.Update(diagram);
        await _context.SaveChangesAsync();
        return StatusCodes.Status200OK;
    }
}