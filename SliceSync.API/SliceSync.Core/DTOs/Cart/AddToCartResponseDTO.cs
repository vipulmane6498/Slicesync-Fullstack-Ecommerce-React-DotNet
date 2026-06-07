using SliceSync.Core.DTOs.CartItem;

namespace SliceSync.Core.DTOs.Cart
{
    public class AddToCartResponseDTO
    {
        public Guid PizzaId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public Decimal TotalCartPrice { get; set; }
    }
}
