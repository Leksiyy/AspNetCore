using homework9.Enum;

namespace homework9.Models;

public class Product
{
    public static int _staitcId = 0;
    public int Id { get; set; } = _staitcId;
    public string Name { get; set; }
    public int Price { get; set; }
    public Categories Category { get; set; }
    public string Comment { get; set; }
}