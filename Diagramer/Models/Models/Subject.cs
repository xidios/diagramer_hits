using Diagramer.Models.Identity;

namespace Diagramer.Models;

public class Subject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public String Name { get; set; }
    public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    public List<Models.Task> Tasks { get; set; } = new List<Models.Task>();
}