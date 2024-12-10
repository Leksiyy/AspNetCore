using homework9.Enum;
using homework9.Models;
using homework9.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace homework9.Controllers;

public class ShopController : Controller
{
    private ProductService _productService;

    public ShopController(ProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var categories = System.Enum.GetValues(typeof(Categories))
            .Cast<Categories>()
            .Select(c => new SelectListItem
            {
                Value = ((int)c).ToString(),
                Text = c.ToString()
            }).ToList();
        ViewBag.Categories = categories;
        var products = _productService.Products.ToList();
        return View(products);
    }

    [HttpPost]
    public IActionResult AddProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _productService.AddProduct(product);
        }
        var products = _productService.Products.ToList();
        return RedirectToAction("Index", products);
    }
}