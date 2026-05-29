using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.Entities
{
    public class Category
    {

        public Guid CategoryId {    get; set; } 

        public string? CategoryType { get; set; }

        public string? CategoryName { get; set; }

        public bool? IsActive { get; set; }

        public ICollection<PizzaCategoryMapping> pizzaCategoryMapping { get; set; } = new List<PizzaCategoryMapping>();
    }
}
