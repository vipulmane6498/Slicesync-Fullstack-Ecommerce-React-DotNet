using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.DTOs.CartItem
{
    public class CartItemResponseDTO
    {
        public Guid CartitemId { get; set; }
        public Guid PizzaId { get; set; }
        public string? PizzaName { get; set; }
        public int Quantity { get; set; }
        public Decimal PriceAtThatTime { get; set; }


    }
}
