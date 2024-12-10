using homework9.Enum;
using homework9.Models;
using homework9.Service;
using homework9.ViewModels;
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
        ViewBag.Categories = System.Enum.GetValues(typeof(Categories))
            .Cast<Categories>()
            .Select(c => new SelectListItem
            {
                Value = ((int)c).ToString(),
                Text = c.ToString()
            }).ToList();
        
        var model = new HomeIndexViewModel
        {
            Products = _productService.Products.ToList(),
            NewProduct = new Product(),
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult AddProduct(HomeIndexViewModel viewModel)
    {
        Product._staitcId++;
        viewModel.NewProduct.Id = Product._staitcId;
        _productService.AddProduct(viewModel.NewProduct);
        
        ViewBag.Categories = System.Enum.GetValues(typeof(Categories))
            .Cast<Categories>()
            .Select(c => new SelectListItem
            {
                Value = ((int)c).ToString(),
                Text = c.ToString()
            }).ToList();
        
        var model = new HomeIndexViewModel
        {
            Products = _productService.Products.ToList(),
            NewProduct = new Product(),
        };
        
        return View("Index", model);
    }

    [HttpGet]
    public IActionResult EditProduct(int id)
    {
        ViewBag.Product = _productService.Products.FirstOrDefault(p => p.Id == id);
        ViewBag.Categories = System.Enum.GetValues(typeof(Categories))
            .Cast<Categories>()
            .Select(c => new SelectListItem
            {
                Value = ((int)c).ToString(),
                Text = c.ToString()
            }).ToList();
        
        return View(new Product());
    }

    [HttpPost]
    public IActionResult EditProduct(Product product)
    {
        _productService.EditProduct(product);
        
        var model = new HomeIndexViewModel
        {
            Products = _productService.Products.ToList(),
            NewProduct = new Product(),
        };
        
        ViewBag.Categories = System.Enum.GetValues(typeof(Categories))
            .Cast<Categories>()
            .Select(c => new SelectListItem
            {
                Value = ((int)c).ToString(),
                Text = c.ToString()
            }).ToList();
        
        return View("Index", model);
    }
}