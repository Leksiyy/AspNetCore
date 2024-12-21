using Microsoft.AspNetCore.Mvc;

namespace homework12.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
