using Diagramer.Models.Identity;

namespace Diagramer.Configuration;

public class ApplicationRoleNames
{
    public const string Admin = "Admin";
    public const string Teacher = "Teacher";
    public const string Student = "Student";

    public static readonly Dictionary<RoleType, String> SystemRoleNamesDictionary = new()
    {
        [RoleType.Administrator] = ApplicationRoleNames.Admin,
        [RoleType.Teacher] = ApplicationRoleNames.Teacher,
        [RoleType.Student] = ApplicationRoleNames.Student
    };

    public const string AdminAndTeacher = Admin + "," + Teacher;
}