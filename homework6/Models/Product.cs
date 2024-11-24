using System.ComponentModel.DataAnnotations;

namespace homework6.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}