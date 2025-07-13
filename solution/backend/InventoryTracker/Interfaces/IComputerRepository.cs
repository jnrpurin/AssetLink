using InventoryTracker.Models;

namespace InventoryTracker.Repositories
{
    public interface IComputerRepository
    {
        Task<IEnumerable<Computer>> GetAllAsync();
        Task<Computer?> GetByIdAsync(int id);
        Task AddAsync(Computer computer);
        Task UpdateAsync(Computer computer);
        Task DeleteAsync(Computer computer);
        Task SaveChangesAsync();
        
        Task<LnkComputerUser?> GetActiveComputerAssignmentAsync(int computerId);
        Task AddComputerAssignmentAsync(LnkComputerUser assignment);
        Task UpdateComputerAssignmentAsync(LnkComputerUser assignment);
        Task<User?> GetUserByIdAsync(int userId);
    }
}
