namespace InventoryTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public DateTime CreateDt { get; set; } = DateTime.UtcNow;

        public ICollection<LnkComputerUser> ComputerUsers { get; set; } = [];
    }
}