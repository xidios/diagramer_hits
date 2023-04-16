using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Diagramer.Models.Hub;

namespace Diagramer.Models.mxGraph;

[XmlRoot(ElementName="mxGraphModel")]
public class MxGraphModel
{
    [XmlIgnore] 
    [Key]
    public Guid MxGraphModelId { get; set; } = Guid.NewGuid();
    [XmlArray("root")]
    [XmlArrayItem("mxCell", typeof(MxCell))]
    public List<MxCell> Cells { get; set; } = new List<MxCell>();
    [XmlIgnore] 
    public Room? Room { get; set; }
    [XmlIgnore] 
    public Guid? RoomId { get; set; }
}