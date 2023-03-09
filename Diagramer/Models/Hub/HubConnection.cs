using Diagramer.Models.Identity;

namespace Diagramer.Models.Hub;

public class HubConnection
{
    public string Id { get; set; }
    public Task Task { get; set; }
    public Guid TaskId { get; set; }
    public ApplicationUser User { get; set; }
    public Guid UserId { get; set; }
    public Group Group { get; set; }
    public Guid GroupId { get; set; }
    public Room Room { get; set; }
    public Guid RoomId { get; set; }
}