using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.Entities
{
    public class CartItem
    {
        public Guid CartItemId { get; set; }    

        [ForeignKey("Cart")]
        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }

        [ForeignKey("Pizza")]
        public Guid PizzaId { get; set; }

        public Pizza? Pizza { get; set; }

        public int Quantity { get; set; }
        public Decimal? PriceAtThatTime { get; set; }
    }
}
