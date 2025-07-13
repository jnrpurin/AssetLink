namespace InventoryTracker.Models
{
    public class Computer
    {
        public int Id { get; set; }
        public int ComputerManufacturerId { get; set; }
        public string SerialNumber { get; set; }
        public string Specifications { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PurchaseDt { get; set; }
        public DateTime WarrantyExpirationDt { get; set; }
        public DateTime CreateDt { get; set; } = DateTime.UtcNow;

        public ComputerManufacturer ComputerManufacturer { get; set; }

        public ICollection<LnkComputerUser> ComputerUsers { get; set; }
        public ICollection<LnkComputerComputerStatus> ComputerStatuses { get; set; }
    }
}