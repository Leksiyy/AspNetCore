using System.Diagnostics;
using Blog.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.Models.Pages;
using Blog.ViewModels;

namespace Blog.Controllers;

public class HomeController : Controller
{
    private readonly IPublication _publication;
    private readonly ICategory _category;
    private readonly IWebHostEnvironment _appEnvironment;
    private readonly ISubscriber _subscribers;

    public HomeController(IPublication publication, ICategory category, IWebHostEnvironment appEnvironment, ISubscriber subscribers)
    {
        _publication = publication;
        _category = category;
        _appEnvironment = appEnvironment;
        _subscribers = subscribers;
    }

    public async Task<IActionResult> Index(QueryOptions? options, string? categoryId)
    {
        var allCategories = await _category.GetAllCategoriesAsync();
        var allPublications = await _publication.GetAllPublicationsWithCategoriesAsync(options, categoryId);

        return View(new IndexViewModel
        {
            Categories = allCategories.ToList(),
            Publications = allPublications,
        });
    }

    [Route("publication")]
    public async Task<IActionResult> GetPublication(string? id, string? returnUrl)
    {
        var currentPublication = await _publication.GetPublicationWithCategoriesAsync(id);
        if (currentPublication is not null)
        {
            await _publication.UpdateViewsAsync(currentPublication.Id.ToString());
            return View(new GetPublicationViewModel
            {
                Publication = currentPublication,
                ReturnUrl = returnUrl
            });
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Subscribe(string email)
    {
        if (!await _subscribers.IsSubscribe(email))
        {
            await _subscribers.Subscribe(new Subscriber
            {
                Email = email,
            });
            return Content("Подписка успешно оформлена!");
        }

        return Content("Вы уже оформили подписку!");
    }
}   