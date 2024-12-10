using homework9.Models;

namespace homework9.Service;

public class ProductService
{
    public List<Product> Products;

    public ProductService()
    {
        Products = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    public void RemoveProduct(Product product)
    {
        Products.Remove(product);
    }
}