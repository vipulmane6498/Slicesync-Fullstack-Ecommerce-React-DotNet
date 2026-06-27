using SliceSync.Core.DTOs.OrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceSync.Core.Enums;

namespace SliceSync.Core.DTOs.Order
{
    public class OrderStatusUpdateResponseDTO
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? OrderChangedAt { get; set; }
        public OrderStatus? PreviousOrderStatus { get; set; }
        public OrderStatus? CurrentOrderStatus { get; set; }
        public string? Message { get; set; }
    }
}
