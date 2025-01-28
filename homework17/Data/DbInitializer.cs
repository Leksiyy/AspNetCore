using homework17.Models;

namespace homework17.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class DbInitializer
{
    public static async Task InitializeAsync(ApplicationContext context, UserManager<Student> userManager)
    {
        await context.Database.MigrateAsync();
        
        if (context.Subjects.Any() || userManager.Users.Any() || context.Grades.Any())
        {
            return; 
        }
        
        var subjects = new List<Subject>
        {
            new Subject { Name = "Mathematics" },
            new Subject { Name = "Physics" },
            new Subject { Name = "History" },
            new Subject { Name = "Computer Science" }
        };

        context.Subjects.AddRange(subjects);
        await context.SaveChangesAsync();
        
        var students = new List<Student>
        {
            new Student
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.doe@example.com",
                Email = "john.doe@example.com",
                Group = "CS-101"
            },
            new Student
            {
                FirstName = "Jane",
                LastName = "Smith",
                UserName = "jane.smith@example.com",
                Email = "jane.smith@example.com",
                Group = "CS-102"
            }
        };

        foreach (var student in students)
        {
            await userManager.CreateAsync(student, "Password123!");
        }
        
        var john = await userManager.FindByEmailAsync("john.doe@example.com");
        var jane = await userManager.FindByEmailAsync("jane.smith@example.com");
        
        var grades = new List<Grade>
        {
            new Grade
            { StudentId = john.Id, SubjectId = subjects[0].Id, Value = 90, Date = DateTime.UtcNow },
            new Grade { StudentId = john.Id, SubjectId = subjects[1].Id, Value = 85, Date = DateTime.UtcNow },
            new Grade { StudentId = jane.Id, SubjectId = subjects[2].Id, Value = 78, Date = DateTime.UtcNow },
            new Grade { StudentId = jane.Id, SubjectId = subjects[3].Id, Value = 92, Date = DateTime.UtcNow }
        };

        context.Grades.AddRange(grades);
        await context.SaveChangesAsync();
    }
}
