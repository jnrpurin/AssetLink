namespace InventoryTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreateDt { get; set; } = DateTime.Now;

        public ICollection<LnkComputerUser> ComputerUsers { get; set; }
    }
}