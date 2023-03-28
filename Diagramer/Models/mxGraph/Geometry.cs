using Diagramer.Models.mxGraph;

public class Geometry
{
    public int CellId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool Relative { get; set; }
    public Point? TargetPoint { get; set; }
    public Point? SourcePoint { get; set; }

    public List<Point> Points { get; set; } = new List<Point>();
    public Point? Offset { get; set; }
}