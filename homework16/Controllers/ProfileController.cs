using homework16.Data;
using homework16.Models;
using homework16.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace homework16.Controllers;

public class ProfileController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public ProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        _context = context;
    }
    
    public IActionResult Index(int page = 1)
    {
        
        int pageSize = 12;
        List<Articles> posts = _context.Articles
            .OrderByDescending(p => p.PublishDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        var totalPosts = _context.Articles.Count();
        var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
        
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        
        var user = _userManager.GetUserAsync(HttpContext.User);

        ProfileViewModel model = new ProfileViewModel
        {
            User = user.Result,
            Articles = posts
        };
        
        return View(model);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);

        return View(user);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(IdentityUser model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }
}