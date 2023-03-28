namespace Diagramer.Models.mxGraph;

public class Cell
{
    public string Id { get; set; }
    public string ParentId { get; set; }
    public bool IsVertex { get; set; } = false;
    public bool IsEdge { get; set; } = false;
    public List<Cell> Children { get; set; }
    public string Style { get; set; }
    public string Value { get; set; }
    public string SourceId { get; set; }
    public string TargetId { get; set; }
    public Geometry Geometry { get; set; }
}