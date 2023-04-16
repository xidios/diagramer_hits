using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Diagramer.Models.mxGraph;

[XmlRoot(ElementName = "mxCell")]
public class MxCell
{
    [XmlIgnore] 
    [Key]
    public Guid MxCellId { get; set; } = Guid.NewGuid();
    [XmlIgnore] 
    public MxGraphModel MxGraphModel { get; set; }
    [XmlIgnore]
    public Guid MxGraphModelId { get; set; }
    [XmlAttribute("id")]
    public string Id { get; set; }
    [XmlAttribute("parent")]
    public string? ParentId { get; set; }
    [XmlAttribute("vertex")]
    public int IsVertex { get; set; }
    [XmlAttribute("edge")]
    public int IsEdge { get; set; }
    //public List<MxCell> Children { get; set; }
    [XmlAttribute("style")]
    public string? Style { get; set; }

    [XmlAttribute("value")] public string? Value { get; set; } = "";
    [XmlAttribute("source")]
    public string? SourceId { get; set; }
    [XmlAttribute("target")]
    public string? TargetId { get; set; }
    [XmlElement("mxGeometry")]
    public MxGeometry? MxGeometry { get; set; }
    [XmlIgnore] 
    public Guid? MxGeometryId { get; set; }
}