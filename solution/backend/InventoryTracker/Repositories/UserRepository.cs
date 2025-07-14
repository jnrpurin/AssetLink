using InventoryTracker.Data;
using InventoryTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InventoryDbContext _context;

        public UserRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
