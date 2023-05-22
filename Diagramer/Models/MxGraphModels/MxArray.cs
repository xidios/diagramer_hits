using System.Xml.Serialization;

namespace Diagramer.Models.mxGraph;

[XmlRoot(ElementName="Array")]
public class MxArray { 

    [XmlIgnore] 
    public Guid MxArrayId { get; set; } = Guid.NewGuid();
    [XmlIgnore] 
    public MxGeometry? MxGeometry { get; set; }
    [XmlIgnore] 
    public Guid? MxGeometryId { get; set; }

    [XmlElement(ElementName = "mxPoint")] public List<MxPoint> MxPoints { get; set; } = new List<MxPoint>();

    [XmlAttribute(AttributeName="as")] 
    public string? As { get; set; } 
}