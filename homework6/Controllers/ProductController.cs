using System.Text.Json;
using homework6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace homework6.Controllers;

public class ProductController : Controller
{
    private readonly string connectionString = "Server=localhost;Database=database;User Id=SA;Password=MisterLeha197618;TrustServerCertificate=True;";
    
    [HttpGet]
    public string Index()
    {
        List<Product> list = new List<Product>();
        using (SqlConnection context = new SqlConnection(connectionString))
        {
            context.Open();
            string query = "select * from Product";
            using (SqlCommand command = new SqlCommand(query, context))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("Id")) && !reader.IsDBNull(reader.GetOrdinal("Name")))
                        {
                            Product person = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                            list.Add(person);
                        }
                    }
                }
            }
        }
        return JsonSerializer.Serialize(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Product (Name) VALUES (@Name)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name ?? (object)DBNull.Value);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        return BadRequest("Product not created");
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public string Search(string keyword)
    {
        List<Product> list = new List<Product>();
        using (SqlConnection context = new SqlConnection(connectionString))
        {
            context.Open();
            string query = "select * from Product where name like @keyword";
            using (SqlCommand command = new SqlCommand(query, context))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("Id")) && !reader.IsDBNull(reader.GetOrdinal("Name")))
                        {
                            Product person = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                            list.Add(person);
                        }
                    }
                }
            }
        }
        return JsonSerializer.Serialize(list);
    }

    public string Details(int id)
    {
        Product person = new Product();
        using (SqlConnection context = new SqlConnection(connectionString))
        {
            context.Open();
            string query = "select * from Product where Id = @id";
            using (SqlCommand command = new SqlCommand(query, context))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        person.Id = reader.GetInt32(reader.GetOrdinal("Id")); //ничего бд не вернет - будет пустой объект
                        person.Name = reader.GetString(reader.GetOrdinal("Name"));
                    }
                }
            }
        }
        return JsonSerializer.Serialize(person);
    }

    [HttpPost]
    public string Delete(int id)
    {
        Product person = new Product();
        using (SqlConnection context = new SqlConnection(connectionString))
        {
            context.Open();
            string query = "select * from Product where Id = @id";
            using (SqlCommand command = new SqlCommand(query, context))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        person.Id = reader.GetInt32(reader.GetOrdinal("Id")); //ничего бд не вернет - будет пустой объект
                        person.Name = reader.GetString(reader.GetOrdinal("Name"));
                    }
                }
            }
            
            using (SqlCommand command = new SqlCommand("DELETE FROM Product where Id = @id", context))
            {
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected <= 0)
                {
                    StatusCode(400);
                    return "Product not deleted";
                }
                else
                {
                    return JsonSerializer.Serialize(person);
                }
            }
        }
    }
}