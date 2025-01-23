using ASPIdentitySelfMaded.Models;
using ASPIdentitySelfMaded.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ASPIdentitySelfMaded.Controllers;

public class UsersController : Controller
{
    private readonly UserManager<User> _userManager;

    public UsersController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public IActionResult Index() => View(_userManager.Users.ToList());
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new User { UserName = model.Email, Email = model.Email, Year = model.Year };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(string id)
    {
        User user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        } 
        EditUserViewModel model = new EditUserViewModel { Id = Convert.ToInt16(user.Id), Email = user.Email, Year = user.Year };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await _userManager.FindByIdAsync(Convert.ToString(model.Id));
            if (user != null)
            {
                user.Email = model.Email;
                user.Year = model.Year;
                user.UserName = model.Email;
                
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        User user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            IdentityResult result = await _userManager.DeleteAsync(user);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ChangePassword(string name)
    {
        User user = await _userManager.FindByNameAsync(name);
        if (user == null)
        {
            return NotFound();
        }
        ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email};
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, "User does not exist.");
                    }
                }
            }
        }
        return View(model);
    }
}