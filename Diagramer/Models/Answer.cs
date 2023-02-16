using Diagramer.Models.Enums;
using Diagramer.Models.Identity;

namespace Diagramer.Models;

public class Answer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }

    public Guid TaskId { get; set; }
    public Models.Task Task { get; set; }
    public Diagram StudentDiagram { get; set; }
    public Diagram? TeacherDiagram { get; set; }
    public AnswerStatusEnum Status { get; set; } = AnswerStatusEnum.InProgress;

    public float? Mark { get; set; } = null;
    public string? Comment { get; set; } = null;
}