using homework12.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using homework12.Services;

namespace homework12.Controllers
{
    public class HomeController : Controller
    {
        private readonly NewsService _newsService;

        public HomeController(NewsService newsService)
        {
            _newsService = newsService;
        }

        public IActionResult Index()
        {
            return View(_newsService.GetNews());
        }

    }
}
