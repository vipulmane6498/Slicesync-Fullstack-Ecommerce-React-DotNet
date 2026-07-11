namespace SliceSync.Core.DTOs.Menu
{
    public class MenuItemResponseDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal UnitPrice { get; set; }
        public List<string> Ingredients { get; set; } = new List<string>();
        public bool SoldOut { get; set; }
        public string? ImageUrl { get; set; }
    }
}
