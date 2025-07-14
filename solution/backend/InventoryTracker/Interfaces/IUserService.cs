using InventoryTracker.Dtos;

namespace InventoryTracker.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllAsync();
        Task<UserReadDto?> GetByIdAsync(int id);
        Task<UserReadDto> CreateAsync(UserCreateDto dto);
        Task<bool> UpdateAsync(int id, UserCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
