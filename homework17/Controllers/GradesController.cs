
using homework17.Data;
using homework17.Models;
using homework17.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace homework17.Controllers;

public class GradesController : Controller
{
    private readonly ApplicationContext _context;
    private readonly UserManager<Student> _userManager;

    public GradesController(ApplicationContext context, UserManager<Student> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }
        
        var subjectsWithGrades = await _context.Subjects
            .Include(s => s.Grades)
            .Where(s => s.Grades.Any(g => g.StudentId == user.Id))
            .Select(s => new SubjectWithGradesViewModel
            {
                SubjectName = s.Name,
                Grades = s.Grades
                    .Where(g => g.StudentId == user.Id)
                    .OrderByDescending(g => g.Date)
                    .Select(g => new GradeViewModel
                    {
                        Value = g.Value,
                        Date = g.Date
                    })
                    .ToList()
            })
            .ToListAsync();

        return View(subjectsWithGrades);
    }
}
