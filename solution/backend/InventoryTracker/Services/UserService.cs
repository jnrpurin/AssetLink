using AutoMapper;
using InventoryTracker.Dtos;
using InventoryTracker.Models;
using InventoryTracker.Repositories;

namespace InventoryTracker.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repository, IMapper mapper, ILogger<UserService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all users from repository.");
            var users = await _repository.GetAllAsync();
            _logger.LogInformation("Mapping {Count} users to DTOs.", users.Count());
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }

        public async Task<UserReadDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching user by ID: {Id} from repository.", id);
            var user = await _repository.GetByIdAsync(id);
            if (user is null)
            {
                _logger.LogWarning("User with ID: {Id} not found in repository.", id);
                return null;
            }
            _logger.LogInformation("Mapping user with ID: {Id} to DTO.", id);
            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<UserReadDto> CreateAsync(UserCreateDto dto)
        {
            _logger.LogInformation("Mapping UserCreateDto to User model for creation. Email: {Email}", dto.EmailAddress);
            var user = _mapper.Map<User>(dto);

            // add logic for the hash psswd here
            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();
            _logger.LogInformation("User created in repository with ID: {Id}.", user.Id);
            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<bool> UpdateAsync(int id, UserCreateDto dto)
        {
            _logger.LogInformation("Attempting to update user with ID: {Id}.", id);
            var user = await _repository.GetByIdAsync(id);
            if (user is null)
            {
                _logger.LogWarning("Update failed: User with ID: {Id} not found for update.", id);
                return false;
            }

            _logger.LogInformation("Mapping UserCreateDto to existing User model ID: {Id}.", id);
            _mapper.Map(dto, user); // Atualiza o objeto existente
            await _repository.UpdateAsync(user);
            await _repository.SaveChangesAsync();
            _logger.LogInformation("User with ID: {Id} updated successfully in repository.", id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Attempting to delete user with ID: {Id}.", id);
            var user = await _repository.GetByIdAsync(id);
            if (user is null)
            {
                _logger.LogWarning("Delete failed: User with ID: {Id} not found for deletion.", id);
                return false;
            }

            await _repository.DeleteAsync(user);
            await _repository.SaveChangesAsync();
            _logger.LogInformation("User with ID: {Id} deleted successfully from repository.", id);
            return true;
        }
    }
}
