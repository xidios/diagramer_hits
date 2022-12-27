using Microsoft.AspNetCore.Identity;

namespace Diagramer.Models.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public String? Name { get; set; }
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public List<Answer> Answers { get; set; } = new List<Answer>();
}