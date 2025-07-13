namespace InventoryTracker.Models
{
    public class ComputerStatus
    {
        public int Id { get; set; }
        public string LocalizedName { get; set; }

        // Propriedade de navegação para a tabela de ligação
        public ICollection<LnkComputerComputerStatus> ComputerStatuses { get; set; }
    }
}