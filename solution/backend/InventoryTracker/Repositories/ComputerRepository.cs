using InventoryTracker.Data;
using InventoryTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Repositories
{
    public class ComputerRepository : IComputerRepository
    {
        private readonly InventoryDbContext _context;

        public ComputerRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Computer>> GetAllAsync()
        {
            return await _context.Computers
                            .Include(c => c.ComputerManufacturer)
                            .Include(c => c.ComputerUsers)
                                .ThenInclude(lcu => lcu.User)
                            .Include(c => c.ComputerStatuses)
                                .ThenInclude(cs => cs.ComputerStatus)
                            .ToListAsync();
        }

        public async Task<Computer?> GetByIdAsync(int id)
        {
            return await _context.Computers
                            .Include(c => c.ComputerManufacturer)
                            .Include(c => c.ComputerUsers)
                                .ThenInclude(lcu => lcu.User)
                            .Include(c => c.ComputerStatuses)
                                .ThenInclude(cs => cs.ComputerStatus)
                            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Computer computer)
        {
            await _context.Computers.AddAsync(computer);
        }

        public Task UpdateAsync(Computer computer)
        {
            _context.Computers.Update(computer);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Computer computer)
        {
            _context.Computers.Remove(computer);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        
        public async Task<LnkComputerUser?> GetActiveComputerAssignmentAsync(int computerId)
        {
            return await _context.LnkComputerUsers
                                 .Where(lcu => lcu.ComputerId == computerId && lcu.AssignEndDt == null)
                                 .OrderByDescending(lcu => lcu.AssignStartDt)
                                 .FirstOrDefaultAsync();
        }

        public async Task AddComputerAssignmentAsync(LnkComputerUser assignment)
        {
            await _context.LnkComputerUsers.AddAsync(assignment);
        }

        public Task UpdateComputerAssignmentAsync(LnkComputerUser assignment)
        {
            _context.LnkComputerUsers.Update(assignment);
            return Task.CompletedTask;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}
