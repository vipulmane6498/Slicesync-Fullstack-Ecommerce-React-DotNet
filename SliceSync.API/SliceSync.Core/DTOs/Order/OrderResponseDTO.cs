using SliceSync.Core.DTOs.OrderItem;
using SliceSync.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.DTOs.Order
{
    public  class OrderResponseDTO
    {
        public Guid OrderId {  get; set; }
        public Guid UserId { get; set; }
        public DateTime? OrderPlacedAt { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public List<OrderItemResonseDTO>? OrderItems { get; set; }

        public Decimal? OrderPrice { get; set; }
    }
}
