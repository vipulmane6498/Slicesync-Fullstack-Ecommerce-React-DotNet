using SliceSync.Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.Entities
{
    public class OrderStatusHistory
    {
        public Guid OrderStatusHistoryId { get; set; }

        [ForeignKey("order")]
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        public string? OrderStatus { get; set; }


        [ForeignKey("ApplicationUser")]
        public Guid? UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        [Column(TypeName ="nvarchar(20)")]
        public string? Role {  get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? Note { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
