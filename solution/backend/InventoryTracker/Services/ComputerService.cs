using AutoMapper;
using InventoryTracker.Dtos;
using InventoryTracker.Models;
using InventoryTracker.Repositories;

namespace InventoryTracker.Services
{
    public class ComputerService : IComputerService
    {
        private readonly IComputerRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ComputerService> _logger;

        public ComputerService(IComputerRepository repository, IMapper mapper, ILogger<ComputerService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ComputerReadDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all computers from repository.");
            var computers = await _repository.GetAllAsync();

            _logger.LogInformation("Mapping {Count} computers to DTOs.", computers.Count());
            return _mapper.Map<IEnumerable<ComputerReadDto>>(computers);
        }

        public async Task<ComputerReadDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching computer by ID: {Id} from repository.", id);
            var computer = await _repository.GetByIdAsync(id);
            if (computer is null)
            {
                _logger.LogWarning("Computer with ID: {Id} not found in repository.", id);
                return null;
            }

            _logger.LogInformation("Mapping computer with ID: {Id} to DTO.", id);
            return _mapper.Map<ComputerReadDto>(computer);
        }

        public async Task<ComputerReadDto> CreateAsync(ComputerCreateDto dto)
        {
            _logger.LogInformation("Mapping ComputerCreateDto to Computer model for creation. SerialNumber: {SerialNumber}", dto.SerialNumber);
            var computer = _mapper.Map<Computer>(dto);
            await _repository.AddAsync(computer);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Computer created in repository with ID: {Id}.", computer.Id);
            return _mapper.Map<ComputerReadDto>(computer);
        }

        public async Task<bool> UpdateAsync(int id, ComputerUpdateDto dto)
        {
            _logger.LogInformation("Attempting to update computer with ID: {Id}.", id);
            var computer = await _repository.GetByIdAsync(id);
            if (computer is null)
            {
                _logger.LogWarning("Update failed: Computer with ID: {Id} not found for update.", id);
                return false;
            }

            _logger.LogInformation("Mapping ComputerUpdateDto to existing Computer model ID: {Id}.", id);
            _mapper.Map(dto, computer);
            await _repository.UpdateAsync(computer);
            await _repository.SaveChangesAsync();
            _logger.LogInformation("Computer with ID: {Id} updated successfully in repository.", id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Attempting to delete computer with ID: {Id}.", id);
            var computer = await _repository.GetByIdAsync(id);
            if (computer is null)
            {
                _logger.LogWarning("Delete failed: Computer with ID: {Id} not found for deletion.", id);
                return false;
            }

            await _repository.DeleteAsync(computer);
            await _repository.SaveChangesAsync();
            _logger.LogInformation("Computer with ID: {Id} deleted successfully from repository.", id);
            return true;
        }
        
        public async Task<bool> AssignComputerToUserAsync(ComputerAssignUserDto dto)
        {
            _logger.LogInformation("Attempting to assign Computer ID: {ComputerId} to User ID: {UserId}.", dto.ComputerId, dto.UserId);

            // 1. Check Computer and user exist
            var computer = await _repository.GetByIdAsync(dto.ComputerId);
            if (computer is null)
            {
                _logger.LogWarning("Assignment failed: Computer with ID: {ComputerId} not found.", dto.ComputerId);
                return false;
            }

            var user = await _repository.GetUserByIdAsync(dto.UserId);
            if (user is null)
            {
                _logger.LogWarning("Assignment failed: User with ID: {UserId} not found.", dto.UserId);
                return false;
            }

            // 2. Handle the opened assignings for the computer
            var existingActiveAssignment = await _repository.GetActiveComputerAssignmentAsync(dto.ComputerId);
            if (existingActiveAssignment != null)
            {
                _logger.LogInformation("Ending previous active assignment for Computer ID: {ComputerId} (from User ID: {PreviousUserId}).", dto.ComputerId, existingActiveAssignment.UserId);
                existingActiveAssignment.AssignEndDt = DateTime.UtcNow; 
                await _repository.UpdateComputerAssignmentAsync(existingActiveAssignment);
            }

            // 3. Create a new assingment
            var newAssignment = _mapper.Map<LnkComputerUser>(dto);
            newAssignment.AssignStartDt = dto.AssignStartDt ?? DateTime.UtcNow; 

            await _repository.AddComputerAssignmentAsync(newAssignment);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Computer ID: {ComputerId} successfully assigned to User ID: {UserId}.", dto.ComputerId, dto.UserId);
            return true;
        }
    }
}
