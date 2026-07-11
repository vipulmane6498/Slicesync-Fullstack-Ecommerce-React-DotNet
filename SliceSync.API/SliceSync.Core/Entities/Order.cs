using SliceSync.Core.Enums;
using SliceSync.Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.Entities
{
    public class Order
    {
        [ForeignKey("ApplicationUser")]
        public Guid? UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public Guid OrderId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? EstimatedDelivery { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public OrderStatus? OrderStatus { get; set; }

        public bool Priority { get; set; } = false;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PriorityPrice { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerAddress { get; set; }
        public string? Position { get; set; }

        public List<OrderItem>? OrderItems { get; set; }
        public List<OrderStatusHistory>? orderStatusHistories { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalOrderPrice { get; set; }
    }
}
