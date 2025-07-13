namespace InventoryTracker.Dtos
{
    public class ComputerReadDto
    {
        public int Id { get; set; } 
        public string SerialNumber { get; set; } = null!; 

        public string ComputerManufacturerName { get; set; } = null!;

        public string Specifications { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime PurchaseDt { get; set; }
        public DateTime WarrantyExpirationDt { get; set; }
        public DateTime CreateDt { get; set; }
        
    }
}
