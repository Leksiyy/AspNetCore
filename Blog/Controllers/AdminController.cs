using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    
    public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    [Route("users")]
    [HttpGet]
    public IActionResult Index() => View(userManager.Users.ToList());

    [Route("edit-user")]
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        User user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        EditUserViewModel model = new EditUserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Phone = user.PhoneNumber
        };
        return View(model);
    }

    [Route("edit-user")]
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.Email = model.Email;
                user.Name = model.Name;
                user.PhoneNumber = model.Phone;
                
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
        }
        return View(model);
    }

    [AutoValidateAntiforgeryToken]
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        User user = await userManager.FindByIdAsync(id);
        if (user != null)
        {
            await userManager.DeleteAsync(user);
        }
        return RedirectToAction("Index");
    }
    
    [Route("user-roles")]
    [HttpGet]
    public async Task<IActionResult> EditRoles(string userId)
    {
        // получаем пользователя
        User user = await userManager.FindByIdAsync(userId);
        if (user != null)
        {
            // получаем список ролей пользователя
            var userRoles = await userManager.GetRolesAsync(user);
            var allRoles = roleManager.Roles.ToList();
            ChangeRoleViewModel model = new ChangeRoleViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserRoles = userRoles,
                AllRoles = allRoles
            };
            return View(model);
        }
        return NotFound();
    }
 
    [Route("user-roles")]
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> EditRoles(string userId, List<string> roles)
    {
        // получаем пользователя
        User user = await userManager.FindByIdAsync(userId);
        if (user != null)
        {
            // получаем список ролей пользователя
            var userRoles = await userManager.GetRolesAsync(user);
            // получаем список ролей, которые были добавлены
            var addedRoles = roles.Except(userRoles);
            // получаем роли, которые были удалены
            var removedRoles = userRoles.Except(roles);
 
            await userManager.AddToRolesAsync(user, addedRoles);
            await userManager.RemoveFromRolesAsync(user, removedRoles);
 
            return RedirectToAction(nameof(Index));
        }
        return NotFound();
    }
}