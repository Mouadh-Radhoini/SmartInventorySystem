using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SmartInventorySystem.Domain.Entities;
using SmartInventorySystem.Domain.Interfaces;
using SmartInventorySystem.Domain.Services;

using SmartInventorySystem.Infrastructure.Database;
using SmartInventorySystem.Infrastructure.Repositories;

namespace SmartInventorySystem.UI
{
    internal static class Program
    {
        public static ServiceProvider Services { get; private set; }
        public static User? LoggedInUser { get; set; }

        [STAThread]
        static void Main()
        {
            var serviceCollection = new ServiceCollection();

            // ===== DATABASE =====
            var connectionString = "Data Source=smartinventory.db";

            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            // ===== REPOSITORIES =====
            serviceCollection.AddTransient<IProductRepository, ProductRepository>();
            serviceCollection.AddTransient<ICategoryRepository, CategoryRepository>();
            serviceCollection.AddTransient<ISaleRepository, SaleRepository>();
            serviceCollection.AddTransient<IUserRepository, UserRepository>();
            serviceCollection.AddTransient<IStockMovementRepository, StockMovementRepository>();

            // ===== SERVICES =====
            serviceCollection.AddTransient<AuthService>();
            serviceCollection.AddTransient<InventoryService>();
            serviceCollection.AddTransient<SalesService>();
            serviceCollection.AddTransient<StockAlertService>();

            // ===== FORMS (IMPORTANT: Transient!) =====
            serviceCollection.AddTransient<LoginForm>();
            serviceCollection.AddTransient<DashboardForm>();
            serviceCollection.AddTransient<ProductsForm>();
            serviceCollection.AddTransient<CategoriesForm>();
            serviceCollection.AddTransient<SalesForm>();
            serviceCollection.AddTransient<LowStockForm>();

            Services = serviceCollection.BuildServiceProvider();

            // ===== CREATE DB & SEED =====
            using (var scope = Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();

                if (!db.Users.Any())
                {
                    db.Users.Add(new User { Username = "admin", Password = "1234", Role = "Admin" });
                    db.Users.Add(new User { Username = "cashier", Password = "1234", Role = "Cashier" });
                    db.SaveChanges();
                }
            }

            ApplicationConfiguration.Initialize();

            // ===== START WITH LOGIN FORM =====
            var login = ActivatorUtilities.CreateInstance<LoginForm>(Services);
            Application.Run(login);
        }
    }
}
