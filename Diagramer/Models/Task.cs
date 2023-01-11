using System.ComponentModel.DataAnnotations;

namespace Diagramer.Models;

public class Task
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public DateTime? Deadline { get; set; } = null;
    public float? Mark { get; set; } = null;
    public string? MarkDescription { get; set; } = null;
    public DateTime? LastResponse { get; set; } = null;
    public Diagram? Diagram { get; set; } = null;
    public List<Category> Categories { get; set; } = new List<Category>();
    public bool IsVisible { get; set; } = false;
    public Subject Subject { get; set; }
    public List<Answer> Answers { get; set; } = new List<Answer>();
}