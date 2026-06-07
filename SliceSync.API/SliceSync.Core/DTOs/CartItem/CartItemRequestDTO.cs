using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.DTOs.CartItem
{
    public class CartItemRequestDTO
    {
        public Guid? PizzaId { get; set; }

        public int Quantity { get; set; }
    }
}
