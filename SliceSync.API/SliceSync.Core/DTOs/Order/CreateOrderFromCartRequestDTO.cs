using System.ComponentModel.DataAnnotations;

namespace SliceSync.Core.DTOs.Order
{
    public class CreateOrderFromCartRequestDTO
    {
        [Required]
        public string? Customer { get; set; }

        [Required]
        public string? Phone { get; set; }

        [Required]
        public string? Address { get; set; }

        public bool Priority { get; set; } = false;

        public string? Position { get; set; }

        [Required]
        public List<FrontendCartItemDTO> Cart { get; set; } = new List<FrontendCartItemDTO>();
    }
}
