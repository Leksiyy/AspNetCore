using homework8.Models;

namespace homework8.ViewModel;

public class BooksViewModel
{
    public List<Book> Books { get; set; } = new List<Book>();

    public Book NewBook { get; set; } = new Book();
}