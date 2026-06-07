using SliceSync.Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.Entities
{
    public class Cart
    {
        public Guid UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public Guid CartId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsActive { get; set; } = false;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

        //when there is no items in the cart it should show Zero(0) cart amount bydefault
        public Decimal? CartPrice { get; set; } = 0;

    }
}
