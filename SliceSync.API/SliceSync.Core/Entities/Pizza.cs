using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.Entities
{
    public class Pizza
    {
        public Guid PizzaId { get; set; }
        public required string PizzaName { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public Decimal Unitprice { get; set; }

   

        public string? Image { get; set; }

        public string? PizzaDesciption { get; set; }

        public bool? IsSoldOut { get; set; }

        public bool? IsActive {get; set;}

        public DateTime? CreateAt { get; set; } = DateTime.Now;

        public required ICollection<PizzaCategoryMapping> pizzaCategoryMapping { get; set; } = new List<PizzaCategoryMapping>();





    }
}
