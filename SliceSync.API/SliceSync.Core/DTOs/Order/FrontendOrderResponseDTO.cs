namespace SliceSync.Core.DTOs.Order
{
    public class FrontendOrderResponseDTO
    {
        public Guid Id { get; set; }
        public string? Status { get; set; }
        public bool Priority { get; set; }
        public decimal PriorityPrice { get; set; }
        public decimal OrderPrice { get; set; }
        public DateTime EstimatedDelivery { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public List<FrontendCartItemDTO> Cart { get; set; } = new List<FrontendCartItemDTO>();
    }
}
