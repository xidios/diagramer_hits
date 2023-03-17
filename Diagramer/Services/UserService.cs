using System.Security.Claims;
using Diagramer.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Diagramer.Services;

public interface IUserService
{
    public Guid GetCurrentUserGuid(ClaimsPrincipal User);
}

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public Guid GetCurrentUserGuid(ClaimsPrincipal User)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            throw new ArgumentNullException("User not found");
        }

        var userGuid = Guid.Parse(userId);
        return userGuid;
    }
}