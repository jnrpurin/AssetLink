namespace InventoryTracker.Models
{
    public class ComputerStatus
    {
        public int Id { get; set; }
        public required string LocalizedName { get; set; }

        public ICollection<LnkComputerComputerStatus> ComputerStatuses { get; set; } = [];
    }
}