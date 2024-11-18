namespace homework3.Models;

record Book(int Id, string Name, string Category)
{
    public Book(string Name, string Category) : this(0, Name, Category)
    {
        
    }
}