using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.Dtos
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string EmailAddress { get; set; } = null!;

        // PasswordHash?
    }
}
