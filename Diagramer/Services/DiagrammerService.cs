using System.Text;
using System.Xml.Serialization;
using Diagramer.Constans;
using Diagramer.Data;
using Diagramer.Models.mxGraph;

namespace Diagramer.Services;

public interface IDiagrammerService
{
    public string ReturnTaskDiagramOrEmpty(string diagramXML);
    public Task<MxGraphModel> CreateMxGraphModelByXML(string? diagramXML);
    public string SerializeMxGraphModelToXML(MxGraphModel graph);
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
}