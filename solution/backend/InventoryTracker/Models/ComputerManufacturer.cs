namespace InventoryTracker.Models
{
    public class ComputerManufacturer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string SerialRegex { get; set; }

        public ICollection<Computer> Computers { get; set; } = [];
    }
}