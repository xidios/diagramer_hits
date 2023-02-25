using Diagramer.Models.Identity;

namespace Diagramer.Models;

public class Group
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public String Name { get; set; }
    public List<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();
    public List<Task> Tasks { get; set; } = new List<Task>();
}