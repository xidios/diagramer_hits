using Diagramer.Data;
using Diagramer.Models;
using Diagramer.Models.Identity;
using Diagramer.Models.ViewModels;
using Diagramer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diagramer.Controllers;

[Route("users")]
[Authorize]
public class UsersController : Controller
{
    private ApplicationDbContext _context;
    private readonly IUserService _userService;
    private readonly IDiagrammerService _diagrammerService;
    private readonly UserManager<ApplicationUser> _userManager;


    public UsersController(ApplicationDbContext context, IUserService userService, IDiagrammerService diagrammerService,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userService = userService;
        _diagrammerService = diagrammerService;
        _userManager = userManager;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users.ToListAsync();

        return View(users);
    }

    [HttpGet]
    [Route("create_group")]
    public async Task<IActionResult> CreateGroup()
    {
        var allUsers = await _context.Users.ToListAsync();
        List<ApplicationUser> students = new List<ApplicationUser>();
        foreach (var user in allUsers)
        {
            if (!await _userManager.IsInRoleAsync(user, "Teacher") && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                students.Add(user);
            }
        }

        return View(new CreateGroupViewModel
        {
            Users = students
        });
    }

    [HttpPost]
    [Route("create_group")]
    public async Task<IActionResult> CreateGroup(CreateGroupViewModel model)
    {
        List<ApplicationUser> selectedUsers =
            await _context.Users.Where(u => model.SelectedUsersId.Any(i => i == u.Id)).ToListAsync();
        Group group = new Group
        {
            Name = model.Name,
            Students = selectedUsers
        };
        foreach (var user in selectedUsers)
        {
            user.Groups.Add(group);
        }

        await _context.Groups.AddAsync(group);
        _context.UpdateRange(selectedUsers);
        await _context.SaveChangesAsync();
        return RedirectToAction("ListOfGroups");
    }

    [Route("groups")]
    public async Task<IActionResult> ListOfGroups()
    {
        return View(await _context.Groups.ToListAsync());
    }
}