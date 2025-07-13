namespace InventoryTracker.Models
{
    public class LnkComputerComputerStatus
    {
        public int Id { get; set; }
        public int ComputerId { get; set; }
        public int ComputerStatusId { get; set; }
        public DateTime AssignDt { get; set; } = DateTime.Now;

        public Computer? Computer { get; set; }
        public ComputerStatus? ComputerStatus { get; set; }
    }
}