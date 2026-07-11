using System.ComponentModel.DataAnnotations.Schema;

namespace SliceSync.Core.DTOs.Order
{
    public class FrontendCartItemDTO
    {
        public Guid PizzaId { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
    }
}
