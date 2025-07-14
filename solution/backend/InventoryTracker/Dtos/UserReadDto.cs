namespace InventoryTracker.Dtos
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public DateTime CreateDt { get; set; }
    }
}
