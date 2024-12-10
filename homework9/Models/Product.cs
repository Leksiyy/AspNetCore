using homework9.Enum;

namespace homework9.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public Categories Category { get; set; }
    public string Comment { get; set; }
}