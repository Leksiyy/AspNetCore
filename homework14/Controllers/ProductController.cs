using homework14.Data;
using homework14.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace homework14.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationContext _context;

    //hardcode
    private readonly List<Product> _products = new List<Product>
    {
        new Product
        {
            Id = 1,
            Name = "Product 1",
            Price = 49.99m, // (USD)
            ImageUrl = "/images/product1.jpg",
            Description = "Description of Product 1"
        },
        new Product
        {
            Id = 2,
            Name = "Product 2",
            Price = 29.99m, // (USD)
            ImageUrl = "/images/product2.jpg",
            Description = "Description of Product 2"
        }
    };
    
    public ProductController(ApplicationContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var selectedCurrency = HttpContext.Request.Cookies["SelectedCurrency"] ?? "USD";

        var currency = _context.Currencies.FirstOrDefault(c => c.Code == selectedCurrency) ?? _context.Currencies.First(c => c.Code == "USD");

        var productsWithConvertedPrices = _products.Select(p => new Product
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            Price = p.Price * currency.Rate,
            Currency = currency.Code
        }).ToList();

        ViewBag.SelectedCurrency = currency.Code;
        ViewBag.Currencies =  _context.Currencies.ToList();
        return View(productsWithConvertedPrices);

    }
    
    [HttpPost]
    public IActionResult SetCurrency(string currency)
    {
        var isCurrencyValid = _context.Currencies.Any(c => c.Code == currency);
        if (!isCurrencyValid)
        {
            return BadRequest("Invalid currency selected.");
        }

        HttpContext.Response.Cookies.Append("SelectedCurrency", currency, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });

        return RedirectToAction("Index");
    }
}