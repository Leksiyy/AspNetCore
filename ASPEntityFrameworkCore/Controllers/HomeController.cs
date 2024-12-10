using System.Diagnostics;
using ASPEntityFrameworkCore.Data;
using Microsoft.AspNetCore.Mvc;
using ASPEntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPEntityFrameworkCore.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationContext _context;

    public HomeController(ApplicationContext context)
    {
        _context = context;
    } 

    public async Task<ActionResult> Index()
    {
        return View(await _context.Users.ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}