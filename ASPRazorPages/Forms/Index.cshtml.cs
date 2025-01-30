using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPRazorPages.Forms;

public class IndexModel : PageModel
{
    public string Message { get; set; }
    public void OnGet()
    {
        Message = "Hello from the Index Page!";
    }
}