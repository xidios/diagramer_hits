using Diagramer.Constans;

namespace Diagramer.Services;

public interface IDiagrammerService
{
    public string ReturnTaskDiagramOrEmpty(string DiagramXML);
}

public class DiagrammerService : IDiagrammerService
{
    public string ReturnTaskDiagramOrEmpty(string? DiagramXML)
    {
        return DiagramXML == null ? DiagramConstans.EMPTY_DIAGRAM_XML : DiagramXML;
    }
}