using Diagramer.Models.mxGraph;

namespace Diagramer.Models.Hub;

public class Room
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Group Group { get; set; }
    public Guid GroupId { get; set; }
    public Task Task { get; set; }
    public Guid TaskId { get; set; }
    public MxGraphModel MxGraphModel { get; set; }
    public Guid MxGraphModelId { get; set; }
    public List<HubConnection> HubConnections { get; set; } = new List<HubConnection>();
}