using homework8.Services;
using homework8.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace homework8.Controllers;

public class BookController : Controller
{
    private readonly BookService _bookService;

    public BookController(BookService bookService)
    {
        _bookService = bookService;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var viewModel = new BooksViewModel { Books = _bookService.GetBooks() };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Add(BooksViewModel model)
    {
        if (ModelState.IsValid)
        {
            _bookService.AddBook(model.NewBook);
        }
        var viewModel = new BooksViewModel
        {
            Books = _bookService.GetBooks()
        };
        return View("Index", viewModel);
    }
}