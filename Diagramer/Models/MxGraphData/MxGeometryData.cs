namespace Diagramer.Models.MxGraphData;

public class MxGeometryData
{
    public string CellId { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public bool Relative { get; set; }
    public MxPointData? Offset { get; set; }
    public MxPointData? SourcePoint { get; set; }
    public MxPointData? TargetPoint { get; set; }
    public List<MxPointData>? Points { get; set; }
}