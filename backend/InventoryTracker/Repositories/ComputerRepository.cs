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
            return await _context.Computers.ToListAsync();
        }

        public async Task<Computer?> GetByIdAsync(int id)
        {
            return await _context.Computers.FindAsync(id);
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
    }
}
