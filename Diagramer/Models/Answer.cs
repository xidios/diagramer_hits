using Diagramer.Models.Identity;

namespace Diagramer.Models;

public class Answer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }

    public Guid TaskId { get; set; }
    public Models.Task Task { get; set; }
    public Diagram Diagram { get; set; }
    public bool Completed { get; set; } = false;
}