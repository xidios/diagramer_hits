namespace Diagramer.Models.ViewModels;

public class CreateTaskViewModel
{
    public Guid Subject_id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public DateTime? Deadline { get; set; } = null;
    public float? Mark { get; set; } = null;
    public string? MarkDescription { get; set; } = null;
    public DateTime? LastResponse { get; set; } = null;
    public string? Diagram { get; set; } = null;
    public bool IsVisible { get; set; } = false;
}