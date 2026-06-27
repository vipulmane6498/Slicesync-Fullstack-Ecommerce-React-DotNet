using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceSync.Core.Enums;

namespace SliceSync.Core.DTOs.Order
{
    public class OrderStatusUpdateRequestDTO
    {
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public string? OrderStatusChangedTo { get; set; }
    }
}
