namespace InventoryTracker.Models
{
    public class LnkComputerUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ComputerId { get; set; }
        public DateTime AssignStartDt { get; set; } = DateTime.Now;
        public DateTime? AssignEndDt { get; set; } = null;

        public User? User { get; set; }
        public Computer? Computer { get; set; }
    }
}