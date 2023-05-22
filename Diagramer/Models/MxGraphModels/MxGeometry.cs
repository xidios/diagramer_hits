using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Diagramer.Models.mxGraph;

//[XmlRoot("mxGeometry")]
public class MxGeometry
{
    [XmlIgnore] 
    [Key]
    public Guid MxGeometryId { get; set; } = Guid.NewGuid();
    [XmlIgnore] 
    public MxCell MxCell { get; set; }
    [XmlIgnore] 
    public Guid MxCellId { get; set; }
    [XmlIgnore] 
    public string? CellId { get; set; }
    [XmlAttribute("x")]
    public float X { get; set; }
    [XmlAttribute("y")]
    public float Y { get; set; }
    [XmlAttribute("height")]
    public float Height { get; set; }
    [XmlAttribute("width")]
    public float Width { get; set; }
    [XmlAttribute("relative")]
    public int Relative { get; set; }
    
    [XmlElement(ElementName="mxPoint")] 
    public List<MxPoint>? Position { get; set; } 
    [XmlElement(ElementName="Array")] 
    public MxArray? Array { get; set; } 
    [XmlAttribute("as")]
    public string As { get; set; }
}