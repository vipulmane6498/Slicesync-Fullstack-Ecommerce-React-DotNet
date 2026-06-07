using System.ComponentModel.DataAnnotations;

namespace SliceSync.Core.DTOs.Cart
{
    public class AddToCartRequestDTO
    {
        [Required(ErrorMessage = "Userid cannot be null")]
        public required Guid UserId { get; set; }
        [Required(ErrorMessage = "Pizzaid cannot be null")]
        public Guid PizzaId { get; set; }
    }
}
