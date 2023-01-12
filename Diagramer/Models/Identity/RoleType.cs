using System.ComponentModel.DataAnnotations;

namespace Diagramer.Models.Identity;

public enum RoleType
{
    [Display(Name = "Administrator")]
    Administrator = 0,
    [Display(Name = "Teacher")]
    Teacher = 1,
    [Display(Name = "Student")]
    Student = 2
}