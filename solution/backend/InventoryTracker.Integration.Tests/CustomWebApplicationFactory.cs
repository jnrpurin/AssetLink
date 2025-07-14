using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; 
using InventoryTracker.Data; 
using InventoryTracker.Models; 
using System.Linq;

namespace InventoryTracker.Integration.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<InventoryDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<InventoryDbContext>(options =>
                {
                    options.UseSqlite("DataSource=file::memory:?cache=shared");
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<InventoryDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                    db.Database.EnsureCreated(); 

                    try
                    {
                        if (!db.ComputerManufacturers.Any())
                        {
                            db.ComputerManufacturers.Add(new ComputerManufacturer { Name = "Dell", SerialRegex = @"^[A-Z0-9]{7,10}$" });
                            db.ComputerManufacturers.Add(new ComputerManufacturer { Name = "HP", SerialRegex = @"^[A-Z0-9]{7,10}$" });
                            db.ComputerManufacturers.Add(new ComputerManufacturer { Name = "Apple", SerialRegex = @"^[A-Z0-9]{7,10}$" });
                            db.SaveChanges();
                        }
                        if (!db.Users.Any())
                        {
                            db.Users.Add(new User { FirstName = "Test", LastName = "User 1", EmailAddress = "test1@example.com" });
                            db.Users.Add(new User { FirstName = "Test", LastName = "User 2", EmailAddress = "test2@example.com" });
                            db.SaveChanges();
                        }
                        if (!db.ComputerStatuses.Any())
                        {
                            db.ComputerStatuses.Add(new ComputerStatus { LocalizedName = "available" });
                            db.ComputerStatuses.Add(new ComputerStatus { LocalizedName = "in_use" });
                            db.ComputerStatuses.Add(new ComputerStatus { LocalizedName = "in_maintenance" });
                            db.ComputerStatuses.Add(new ComputerStatus { LocalizedName = "retired" });
                            db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database with test data.");
                    }
                }
            });
        }
    }
}
