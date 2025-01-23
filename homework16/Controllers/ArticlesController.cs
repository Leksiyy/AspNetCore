using homework16.Data;
using homework16.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace homework16.Controllers;

public class ArticlesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ArticlesController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IActionResult Index(int page = 1)
    {
        int pageSize = 5;
        List<Articles> posts = _context.Articles
            .OrderByDescending(p => p.PublishDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        var totalPosts = _context.Articles.Count();
        var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
        
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View(posts);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        return View(new Articles());
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Articles article)
    {
        if (ModelState.IsValid)
        {
            _context.Articles.Add(article);
            _context.SaveChanges();
            return RedirectToAction("Index", "Articles");
        }

        return BadRequest();
    }

    public IActionResult Details(int id)
    {
        var article = _context.Articles.Find(id);
        return View(article);
    }
}