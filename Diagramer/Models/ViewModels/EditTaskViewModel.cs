namespace Diagramer.Models.ViewModels;

public class EditTaskViewModel
{
    public Guid? TaskId { get; set; } = null;
    public List<Category> Categories { get; set; }

    public List<Category> SelectedCategories { get; set; }
    public List<Guid> CategoriesIds { get; set; } = new List<Guid>();
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? Deadline { get; set; } = null;
    public float? Mark { get; set; } = null;
    public string? MarkDescription { get; set; } = null;
    public string? Diagram { get; set; } = null;
    public bool IsVisible { get; set; } = false;
}