using System.Xml.Serialization;

namespace Diagramer.Models.mxGraph;

[XmlRoot(ElementName="mxPoint")]
public class MxPoint
{
    [XmlIgnore] 
    public Guid MxPointId { get; set; } = Guid.NewGuid();
    [XmlIgnore] 
    public MxGeometry? MxGeometry { get; set; }
    [XmlIgnore] 
    public Guid? MxGeometryId { get; set; }
    [XmlIgnore] 
    public MxArray? MxArray { get; set; }
    [XmlIgnore] 
    public Guid? MxArrayId { get; set; }
    [XmlAttribute("x")]
    public float X { get; set; }
    [XmlAttribute("y")]
    public float Y { get; set; }
    [XmlAttribute("as")]
    public string? As { get; set; }
}