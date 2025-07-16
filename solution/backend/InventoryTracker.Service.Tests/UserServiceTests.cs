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
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UserService>> _mockLogger; 
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UserService>>(); 

            _userService = new UserService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, FirstName = "Test", LastName = "User1", EmailAddress = "test1@example.com" },
                new User { Id = 2, FirstName = "Test", LastName = "User2", EmailAddress = "test2@example.com" }
            };
            var userReadDtos = new List<UserReadDto>
            {
                new UserReadDto { Id = 1, FirstName = "Test", LastName = "User1", EmailAddress = "test1@example.com" },
                new UserReadDto { Id = 2, FirstName = "Test", LastName = "User2", EmailAddress = "test2@example.com" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);
            _mockMapper.Setup(m => m.Map<IEnumerable<UserReadDto>>(It.IsAny<IEnumerable<User>>()))
                       .Returns(userReadDtos);

            // Act
            var result = await _userService.GetAllAsync();

            // Assert
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<UserReadDto>>(users), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(userReadDtos.Count, result.Count());
        }
    }
}
