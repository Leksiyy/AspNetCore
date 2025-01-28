using homework17.Data;
using homework17.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace homework17.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationContext _context;
    private readonly UserManager<Student> _userManager;
    private readonly List<string> _allowedEmails;

    public AdminController(ApplicationContext context, UserManager<Student> userManager, IOptions<AppSettings> options)
    {
        _context = context;
        _userManager = userManager;
        _allowedEmails = options.Value.TeacherAccounts;
    }

    private async Task<bool> IsUserAllowed()
    {
        var user = await _userManager.GetUserAsync(User);
        return user != null && _allowedEmails.Contains(user.Email);
    }

    [HttpGet]
    public async Task<IActionResult> CreateSubject()
    {
        if (!await IsUserAllowed())
        {
            return RedirectToAction("AccessDenied", "Account");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSubject(string subjectName)
    {
        if (!await IsUserAllowed())
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        if (string.IsNullOrEmpty(subjectName))
        {
            ModelState.AddModelError(string.Empty, "Subject name cannot be empty.");
            return View();
        }

        var subject = new Subject { Name = subjectName };
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> AssignGrade()
    {
        if (!await IsUserAllowed())
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        ViewBag.Students = await _context.Users.ToListAsync();
        ViewBag.Subjects = await _context.Subjects.ToListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignGrade(int subjectId, string studentId, int gradeValue)
    {
        if (!await IsUserAllowed())
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        var student = await _context.Users.FindAsync(studentId);
        var subject = await _context.Subjects.FindAsync(subjectId);

        if (student == null || subject == null || gradeValue < 1 || gradeValue > 100)
        {
            ModelState.AddModelError(string.Empty, "Invalid data.");
            ViewBag.Students = await _context.Users.ToListAsync();
            ViewBag.Subjects = await _context.Subjects.ToListAsync();
            return View();
        }

        var grade = new Grade
        {
            StudentId = student.Id,
            SubjectId = subject.Id,
            Value = gradeValue,
            Date = DateTime.Now
        };

        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }
}
