using InventoryTracker.Dtos;

namespace InventoryTracker.Services
{
    public interface IComputerService
    {
        Task<IEnumerable<ComputerReadDto>> GetAllAsync();
        Task<ComputerReadDto?> GetByIdAsync(int id);
        Task<ComputerReadDto> CreateAsync(ComputerCreateDto dto);
        Task<bool> UpdateAsync(int id, ComputerUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
