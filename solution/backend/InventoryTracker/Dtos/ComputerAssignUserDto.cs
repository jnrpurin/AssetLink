using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.Dtos
{
    public class ComputerAssignUserDto
    {
        [Required]
        public int ComputerId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime? AssignStartDt { get; set; }
    }
}
