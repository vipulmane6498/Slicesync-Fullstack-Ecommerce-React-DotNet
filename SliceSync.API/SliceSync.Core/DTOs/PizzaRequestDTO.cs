using SliceSync.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.DTOs
{
    public class PizzaRequestDTO
    {
        //public int PizzaId { get; set; }
        public string PizzaName { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Unitprice { get; set; }



        public string? ImageUrl { get; set; }

        public string? PizzaDesciption { get; set; }

        public bool? IsSoldOut { get; set; }

        public bool? IsActive { get; set; }

        //public DateTime? CreateAt { get; set; } = DateTime.Now;

        public List<Guid> CategoryId { get; set; } = new List<Guid>();

       
    }
}
