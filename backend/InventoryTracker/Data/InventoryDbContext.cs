using InventoryTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options) { }

        public DbSet<ComputerManufacturer> ComputerManufacturers { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ComputerStatus> ComputerStatuses { get; set; }
        public DbSet<LnkComputerUser> LnkComputerUsers { get; set; }
        public DbSet<LnkComputerComputerStatus> LnkComputerComputerStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LnkComputerUser>()
                .HasOne(lcu => lcu.User)
                .WithMany(u => u.ComputerUsers)
                .HasForeignKey(lcu => lcu.UserId);

            modelBuilder.Entity<LnkComputerUser>()
                .HasOne(lcu => lcu.Computer)
                .WithMany(c => c.ComputerUsers)
                .HasForeignKey(lcu => lcu.ComputerId);

            modelBuilder.Entity<LnkComputerComputerStatus>()
                .HasOne(lccc => lccc.Computer)
                .WithMany(c => c.ComputerStatuses)
                .HasForeignKey(lccc => lccc.ComputerId);

            modelBuilder.Entity<LnkComputerComputerStatus>()
                .HasOne(lccc => lccc.ComputerStatus)
                .WithMany(cs => cs.ComputerStatuses)
                .HasForeignKey(lccc => lccc.ComputerStatusId);

            modelBuilder.Entity<Computer>()
                .Property(c => c.CreateDt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<User>()
                .Property(u => u.CreateDt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<LnkComputerUser>()
                .Property(lcu => lcu.AssignStartDt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<LnkComputerComputerStatus>()
                .Property(lccc => lccc.AssignDt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
