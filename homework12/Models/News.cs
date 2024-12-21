using System.ComponentModel.DataAnnotations;

namespace homework12.Models;

public class News
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
}