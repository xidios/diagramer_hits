using Diagramer.Models.Identity;

namespace Diagramer.Models.ViewModels;

public class AddUsersToSubjectViewModel
{
    public Guid? SubjectId { get; set; }= null;
    public List<ApplicationUser> Users { get; set; }
    public List<Guid> SelectedUsersId { get; set; }
}