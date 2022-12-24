using Microsoft.AspNetCore.Identity;

namespace Diagramer.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public String? Name { get; set; }
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}