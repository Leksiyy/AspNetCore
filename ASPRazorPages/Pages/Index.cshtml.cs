using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPRazorPages.Pages;

public class IndexModel : PageModel
{
    public string Message { get; set; }
    private readonly decimal currentRate = 38.1m;

    public void OnGet()
    {
        Message = "Enter the amount";
    }

    public void OnPost(int? sum)
    {
        if (sum == null || sum <= 1000)
        {
            Message = "An incorrect amount was transmitted. Repeat the entry.";
        }
        else
        {
            decimal result = sum.Value / currentRate;
            Message = $"In exchange {sum} of hryvnas you'll get {result:F2} dollars";
        }
    }
}