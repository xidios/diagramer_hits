using Diagramer.Models.Identity;

namespace Diagramer.Models.ViewModels;

public class CreateGroupViewModel
{
    public string Name { get; set; }
    public List<ApplicationUser> Users { get; set; }
    public List<Guid> SelectedUsersId { get; set; }
}