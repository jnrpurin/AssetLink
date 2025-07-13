namespace InventoryTracker.Models
{
    public class ComputerManufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SerialRegex { get; set; }

        public ICollection<Computer> Computers { get; set; }
    }
}