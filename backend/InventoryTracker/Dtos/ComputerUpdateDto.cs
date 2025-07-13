using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.Dtos
{
    public class ComputerUpdateDto
    {
        [Required]
        public int ComputerManufacturerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SerialNumber { get; set; } = null!;

        [MaxLength(500)]
        public string Specifications { get; set; } = null!;

        [Url]
        [MaxLength(200)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public DateTime PurchaseDt { get; set; }

        public DateTime WarrantyExpirationDt { get; set; }
    }
}
