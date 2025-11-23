using SmartInventorySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SmartInventorySystem.Infrastructure.Database
{
    public static class AppDbContextSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.Migrate();

            // Seed Admin User
            if (!context.Users.Any())
            {
                var admin = new User
                {
                    Username = "admin",
                    Password = "admin",
                    Role = "Admin"
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }

            // Seed Categories
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Electronics" },
                    new Category { Name = "Groceries" },
                    new Category { Name = "Clothes" }
                );

                context.SaveChanges();
            }

            // Seed Products
            if (!context.Products.Any())
            {
                var electronics = context.Categories.First(c => c.Name == "Electronics");

                context.Products.AddRange(
                    new Product { Name = "Keyboard", Price = 49.99M, Quantity = 10, CategoryId = electronics.Id },
                    new Product { Name = "Mouse", Price = 19.99M, Quantity = 20, CategoryId = electronics.Id }
                );

                context.SaveChanges();
            }
        }
    }
}