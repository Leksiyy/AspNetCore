using Blog.Interfaces;
using Blog.Models;
using Blog.Models.Pages;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Controllers;

[Authorize(Roles = "Admin,Editor")]
public class ContentController : Controller
{
    private readonly ICategory _categories;
    private readonly IPublication _publication;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ContentController(ICategory categories, IPublication publication, IWebHostEnvironment webHostEnvironment)
    {
        _categories = categories;
        _publication = publication;
        _webHostEnvironment = webHostEnvironment;
    }

    [Route("content")]
    public async Task<IActionResult> Index(QueryOptions options, string id)
    {
        return View(new ContentViewModel
        {
            Categories = await _categories.GetAllCategoriesAsync(),
            Publications = await _publication.GetAllPublicationsWithCategoriesAsync(options, id)
        });
    }
    
    #region Categories
    [Route("create-category")]
    public IActionResult CreateCategory()
    {
        return View();
    }
 
    [Route("create-category")]
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> CreateCategory(CategoryViewModel categoryViewModel)
    {
        if (ModelState.IsValid)
        {
            await _categories.AddCategoryAsync(new Category
            {
                Name = categoryViewModel.Name,
                Description = categoryViewModel.Description
            });
            return RedirectToAction(nameof(Index));
        }
        return View(categoryViewModel);
    }
 
    [Route("edit-category")]
    public async Task<IActionResult> EditCategory(string id)
    {
        var currentCategory = await _categories.GetCategoryAsync(id);
        if (currentCategory is not null)
        {
            return View(new CategoryViewModel
            {
                Id = currentCategory.Id,
                Name = currentCategory.Name,
                Description = currentCategory.Description
            });
        }
        return NotFound();
    }
 
    [Route("edit-category")]
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> EditCategory(CategoryViewModel categoryViewModel)
    {
        if (ModelState.IsValid)
        {
            await _categories.UpdateCategoryAsync(new Category
            {
                Id = categoryViewModel.Id,
                Name = categoryViewModel.Name,
                Description = categoryViewModel.Description
            });
            return RedirectToAction(nameof(Index));
        }
        return View(categoryViewModel);
    }
 
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        var currentCategory = await _categories.GetCategoryAsync(id);
        if (currentCategory != null)
        {
            await _categories.DeleteCategoryAsync(currentCategory);
        }
        return RedirectToAction(nameof(Index));
    }
    #endregion
    
    #region Publications
    [Route("create-publication")]
    public async Task<IActionResult> CreatePublication()
    {
        var allCategories = await _categories.GetAllCategoriesAsync();
     
        var categoriesList = allCategories.Select(e => new SelectListItem
        {
            Text = e.Name,
            Value = e.Id.ToString()
        });
     
        return View(new PublicationViewModel
        {
            SelectListCategories = categoriesList
        });
    }
     
    [Route("create-publication")]
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> CreatePublication(PublicationViewModel publicationViewModel, string[] categories)
    {
        if (ModelState.IsValid)
        {
            string? fileImageName = null, imagePath = null;
            if (publicationViewModel.File != null)
            {
                fileImageName = publicationViewModel.File.FileName;
     
                if (fileImageName.Contains("\\"))
                {
                    fileImageName = fileImageName.Substring(fileImageName.LastIndexOf('\\') + 1);
                }
     
                imagePath = "/publicationImages/" + Guid.NewGuid() + fileImageName;
     
                using (var fileStream = new FileStream(_webHostEnvironment.WebRootPath + imagePath, FileMode.Create))
                {
                    await publicationViewModel.File.CopyToAsync(fileStream);
                }
            }
     
            await _publication.AddPublicationAsync(new Publication
            {
                Title = publicationViewModel.Title,
                Description = publicationViewModel.Description,
                SeoDescription = publicationViewModel.SeoDescription,
                Keywords = publicationViewModel.Keywords,
                FullImageName = fileImageName,
                Image = imagePath,
                Categories = categories.Select(e => new Category
                {
                    Id = new Guid(e)
                }).ToList()
            });
            return RedirectToAction(nameof(Index));
        }
        var allCategories = await _categories.GetAllCategoriesAsync();
     
        publicationViewModel.SelectListCategories = allCategories.Select(e => new SelectListItem
        {
            Text = e.Name,
            Value = e.Id.ToString()
        });
     
        return View(publicationViewModel);
    }
     
    [Route("edit-publication")]
    public async Task<IActionResult> EditPublication(string id)
    {
        var currentPublication = await _publication.GetPublicationWithCategoriesAsync(id);
     
        if (currentPublication != null)
        {
            var allCategories = await _categories.GetAllCategoriesAsync();
            var categoryIds = currentPublication.Categories.Select(e => e.Id.ToString()).ToArray();
     
            var categoriesList = allCategories.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString()
            }).ToList();
     
            foreach (var item in categoriesList)
            {
                if (categoryIds.Contains(item.Value))
                {
                    item.Selected = true;
                }
            }
     
            return View(new PublicationViewModel
            {
                Id = currentPublication.Id,
                Title = currentPublication.Title,
                Description = currentPublication.Description,
                SeoDescription = currentPublication.SeoDescription,
                Keywords = currentPublication.Keywords,
                Image = currentPublication.Image,
                ImageFullName = currentPublication.FullImageName,
                SelectListCategories = categoriesList
            });
        }
        return RedirectToAction(nameof(Index));
    }
     
    [Route("edit-publication")]
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> EditPublication(PublicationViewModel publicationViewModel, string[] categories)
    {
        if (ModelState.IsValid)
        {
            var currentPublication = await _publication.GetPublicationAsync(publicationViewModel.Id.ToString());
            if (currentPublication == null) { return NotFound(); }
            string? fileImageName = null, imagePath = null;
            if (publicationViewModel.File != null)
            {
                if (System.IO.File.Exists(_webHostEnvironment.WebRootPath + currentPublication.Image))
                {
                    System.IO.File.Delete(_webHostEnvironment.WebRootPath + currentPublication.Image);
                }
     
                fileImageName = publicationViewModel.File.FileName;
     
                if (fileImageName.Contains("\\"))
                {
                    fileImageName = fileImageName.Substring(fileImageName.LastIndexOf('\\') + 1);
                }
     
                imagePath = "/publicationImages/" + Guid.NewGuid() + fileImageName;
     
                using (var fileStream = new FileStream(_webHostEnvironment.WebRootPath + imagePath, FileMode.Create))
                {
                    await publicationViewModel.File.CopyToAsync(fileStream);
                }
            }
            else
            {
                fileImageName = currentPublication.FullImageName;
                imagePath = currentPublication.Image;
            }
     
            await _publication.UpdatePublicationAsync(new Publication
            {
                Id = currentPublication.Id,
                Title = publicationViewModel.Title,
                Description = publicationViewModel.Description,
                SeoDescription = publicationViewModel.SeoDescription,
                Keywords = publicationViewModel.Keywords,
                FullImageName = fileImageName,
                Image = imagePath,
                Categories = categories.Select(e => new Category
                {
                    Id = new Guid(e)
                }).ToList()
            });
            return RedirectToAction(nameof(Index));
        }
        var allCategories = await _categories.GetAllCategoriesAsync();
     
        publicationViewModel.SelectListCategories = allCategories.Select(e => new SelectListItem
        {
            Text = e.Name,
            Value = e.Id.ToString()
        });
     
        return View(publicationViewModel);
    }
     
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> DeletePublication(string id)
    {
        var currentPublication = await _publication.GetPublicationAsync(id);
        if (currentPublication != null)
        {
            await _publication.DeletePublicationAsync(currentPublication);
     
            if (System.IO.File.Exists(_webHostEnvironment.WebRootPath + currentPublication.Image))
            {
                System.IO.File.Delete(_webHostEnvironment.WebRootPath + currentPublication.Image);
            }
        }
        return RedirectToAction(nameof(Index));
    }
    #endregion
}