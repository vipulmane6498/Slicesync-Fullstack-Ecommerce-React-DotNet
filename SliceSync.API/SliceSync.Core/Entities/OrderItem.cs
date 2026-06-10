using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.Entities
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }


        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }


        [ForeignKey("Pizza")]
        public Guid PizzaId { get; set; }

        public string? PizzaName { get; set; }
        public Pizza? Pizza { get; set; }

        public int Quantity { get; set; }

        public Decimal? PriceAtThatTime { get; set; } = 0;

    }
}
