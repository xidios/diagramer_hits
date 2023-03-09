using Diagramer.Models.Identity;

namespace Diagramer.Constans;

public static class HTMLHelpers
{
    public static string NameOfUser(ApplicationUser? user)
    {
        if (user == null)
        {
            return "Пользователь не найден";
        }
        if (user.Name == null)
            return $"Имя не указано, почта: {user.Email}";
        return user.Name;
    }
}