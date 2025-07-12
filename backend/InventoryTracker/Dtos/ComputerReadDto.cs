namespace InventoryTracker.Dtos
{
    public class ComputerReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string SerialNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
