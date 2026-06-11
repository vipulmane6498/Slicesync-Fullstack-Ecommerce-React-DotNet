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
        public Guid UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public Guid OrderId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        public OrderStatus? OrderStatus { get; set; }

        public List<OrderItem>? OrderItems { get; set; }

        public Decimal? TotalOrderPrice { get; set; }
        
    }
}
