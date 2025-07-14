using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using InventoryTracker.Dtos;
using InventoryTracker.Models;
using InventoryTracker.Data;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Integration.Tests
{
    public class ComputersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public ComputersControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(); 
        }

        private async Task SeedDatabaseAsync(Action<InventoryDbContext> seedAction)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                seedAction(dbContext);
                await dbContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task GetAllComputers_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange
            await SeedDatabaseAsync(db =>
            {
                db.ComputerManufacturers.Add(new ComputerManufacturer { Id = 1, Name = "TestBrand", SerialRegex = @"^[A-Z0-9]{7,10}$" });
                db.Computers.Add(new Computer
                {
                    Id = 1,
                    ComputerManufacturerId = 1,
                    SerialNumber = "SN-INT-001",
                    Specifications = "Integration Test Spec",
                    ImageUrl = "http://test.com/img.jpg",
                    PurchaseDt = DateTime.UtcNow,
                    WarrantyExpirationDt = DateTime.UtcNow.AddYears(1)
                });
            });

            // Act
            var response = await _client.GetAsync("/api/Computers");

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var computers = await response.Content.ReadFromJsonAsync<IEnumerable<ComputerReadDto>>();
            Assert.NotNull(computers);
            Assert.Single(computers); 
            Assert.Equal("SN-INT-001", computers.First().SerialNumber);
            Assert.Equal("TestBrand", computers.First().ComputerManufacturerName); 
        }

        [Fact]
        public async Task CreateComputer_ReturnsCreatedAndComputerReadDto()
        {
            await SeedDatabaseAsync(db =>
            {
                db.ComputerManufacturers.Add(new ComputerManufacturer { Id = 10, Name = "NewBrand", SerialRegex = @"^[A-Z0-9]{7,10}$" });
            });

            var createDto = new ComputerCreateDto
            {
                ComputerManufacturerId = 10,
                SerialNumber = "NEW-INT-002",
                Specifications = "New Integration Test Spec",
                ImageUrl = "http://new.com/img.jpg",
                PurchaseDt = DateTime.UtcNow,
                WarrantyExpirationDt = DateTime.UtcNow.AddYears(2)
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Computers", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(HttpStatusCode.Created, response.StatusCode); 
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var createdComputer = await response.Content.ReadFromJsonAsync<ComputerReadDto>();
            Assert.NotNull(createdComputer);
            Assert.True(createdComputer.Id > 0); 
            Assert.Equal(createDto.SerialNumber, createdComputer.SerialNumber);
            Assert.Equal("NewBrand", createdComputer.ComputerManufacturerName);
        }

        [Fact]
        public async Task GetComputerById_ReturnsNotFound_WhenComputerDoesNotExist()
        {
            await SeedDatabaseAsync(db => {});

            // Act
            var response = await _client.GetAsync("/api/Computers/999"); 

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AssignComputer_ReturnsOk_WhenAssignmentIsSuccessful()
        {
            await SeedDatabaseAsync(db =>
            {
                db.ComputerManufacturers.Add(new ComputerManufacturer { Id = 1, Name = "TestBrand", SerialRegex = @"^[A-Z0-9]{7,10}$" });
                db.Computers.Add(new Computer
                {
                    Id = 100,
                    ComputerManufacturerId = 1,
                    SerialNumber = "SN-ASSIGN-001",
                    Specifications = "Assign Test Spec",
                    ImageUrl = "http://assign.com/img.jpg",
                    PurchaseDt = DateTime.UtcNow,
                    WarrantyExpirationDt = DateTime.UtcNow.AddYears(1)
                });
                db.Users.Add(new User { Id = 101, FirstName = "Assign", LastName = "User", EmailAddress = "assign@example.com" });
            });

            var assignDto = new ComputerAssignUserDto
            {
                ComputerId = 100,
                UserId = 101,
                AssignStartDt = DateTime.UtcNow
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(assignDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Computers/assign", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
                var assignment = await dbContext.LnkComputerUsers.FirstOrDefaultAsync(lcu => lcu.ComputerId == 100 && lcu.UserId == 101);
                Assert.NotNull(assignment);
                Assert.Null(assignment.AssignEndDt); 
            }
        }
    }
}
