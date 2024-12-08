using homework8.Models;

namespace homework8.Services;

public class BookService
{
    private List<Book> _books;

    public BookService()
    {
        _books = new List<Book>();
    }

    public List<Book> GetBooks()
    {
        return _books;
    }

    public void AddBook(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));
        
        _books.Add(book);
    }

    public bool RemoveBook(int bookId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);
        if (book != null)
        {
            _books.Remove(book);
            return true;
        }

        return false;
    }

    public Book GetBookById(int bookId)
    {
        return _books.FirstOrDefault(b => b.Id == bookId);
    }

    public bool UpdateBook(int bookId, Book updatedBook)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);
        if (book != null)
        {
            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Year = updatedBook.Year;

            return true;
        }

        return false;
    }
}