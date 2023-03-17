namespace Diagramer.Models.ViewModels;

public class AddGroupsToTaskViewModel
{
    public Guid? TaskId { get; set; } = null;
    public List<Group> Groups { get; set; }
    public List<Guid> SelectedGroupsId { get; set; }
}