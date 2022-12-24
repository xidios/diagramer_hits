using Diagramer.Models.Identity;

namespace Diagramer.Models;

public class Subject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}