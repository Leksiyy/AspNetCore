using System;

namespace homework10.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string Date { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
}