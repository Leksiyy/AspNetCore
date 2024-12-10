using homework10.Models;

namespace homework10.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider) // удобный интерфейс подсмотрел
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            
            if (context.Books.Any())
            {
                return;
            }

            context.Books.AddRange(
                new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Genre = "Fiction", Price = 10 },
                new Book { Title = "1984", Author = "George Orwell", Genre = "Dystopian", Price = 15 },
                new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Genre = "Fiction", Price = 12 },
                new Book { Title = "Pride and Prejudice", Author = "Jane Austen", Genre = "Romance", Price = 14 },
                new Book { Title = "The Catcher in the Rye", Author = "J.D. Salinger", Genre = "Fiction", Price = 11 }
            );

            context.SaveChanges();
        }
    }
}