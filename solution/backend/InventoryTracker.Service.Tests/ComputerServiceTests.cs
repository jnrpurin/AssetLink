using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryTracker.Models;
using InventoryTracker.Dtos;
using InventoryTracker.Repositories;
using InventoryTracker.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace InventoryTracker.Service.Tests
{
    public class ComputerServiceTests
    {
        private readonly Mock<IComputerRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<ComputerService>> _mockLogger; 
        private readonly ComputerService _computerService;

        public ComputerServiceTests()
        {
            _mockRepository = new Mock<IComputerRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<ComputerService>>(); 

            _computerService = new ComputerService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllComputers()
        {
            // Arrange
            var computers = new List<Computer>
            {
                new Computer {
                    Id = 1,
                    SerialNumber = "SN001",
                    ComputerManufacturerId = 1,
                    Specifications = "Test Specs 1",
                    ImageUrl = "http://test.com/img1.jpg",
                    PurchaseDt = DateTime.Now,
                    WarrantyExpirationDt = DateTime.Now.AddYears(1)
                },
                new Computer {
                    Id = 2,
                    SerialNumber = "SN002",
                    ComputerManufacturerId = 2,
                    Specifications = "Test Specs 2",
                    ImageUrl = "http://test.com/img2.jpg",
                    PurchaseDt = DateTime.Now,
                    WarrantyExpirationDt = DateTime.Now.AddYears(1)
                }
            };
            var computerReadDtos = new List<ComputerReadDto>
            {
                new ComputerReadDto { Id = 1, SerialNumber = "SN001", ComputerManufacturerName = "BrandA" },
                new ComputerReadDto { Id = 2, SerialNumber = "SN002", ComputerManufacturerName = "BrandB" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(computers);
            _mockMapper.Setup(m => m.Map<IEnumerable<ComputerReadDto>>(It.IsAny<IEnumerable<Computer>>()))
                       .Returns(computerReadDtos);

            // Act
            var result = await _computerService.GetAllAsync();

            // Assert
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ComputerReadDto>>(It.IsAny<IEnumerable<Computer>>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(computerReadDtos.Count, result.Count());
            Assert.Equal(computerReadDtos.First().Id, result.First().Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenComputerDoesNotExist()
        {
            // Arrange
            var computerId = 99;
            _mockRepository.Setup(repo => repo.GetByIdAsync(computerId)).ReturnsAsync((Computer?)null);

            // Act
            var result = await _computerService.GetByIdAsync(computerId);

            // Assert
            _mockRepository.Verify(repo => repo.GetByIdAsync(computerId), Times.Once);
            _mockMapper.Verify(m => m.Map<ComputerReadDto>(It.IsAny<Computer>()), Times.Never); 
            Assert.Null(result);
        }
    }
}
