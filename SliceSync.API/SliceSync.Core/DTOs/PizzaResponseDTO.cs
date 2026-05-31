using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.DTOs
{
    public class PizzaResponseDTO
    {
        public Guid PizzaId { get; set; }
        public string PizzaName { get; set; }
        public decimal Unitprice { get; set; }
        public string Image { get; set; }
        public string PizzaDesciption { get; set; }
        public bool IsSoldOut { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public List<CategoryResonseDTO> Categories { get; set; }
    }
}
