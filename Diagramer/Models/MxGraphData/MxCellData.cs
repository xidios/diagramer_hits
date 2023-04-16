namespace Diagramer.Models.MxGraphData;

public class MxCellData
{
    public List<MxCellData>? Children { get; set; }
    public string Id { get; set; }
    public string ParentId { get; set; }
    public string? Style { get; set; }
    public string? Value { get; set; }
    public MxGeometryData Geometry { get; set; }
    public bool IsVertex { get; set; }
    public bool IsEdge { get; set; }
    public string? SourceId { get; set; }
    public string? TargetId { get; set; }
}