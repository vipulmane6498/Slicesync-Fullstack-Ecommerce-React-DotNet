using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.DTOs
{
    public class CategoryResonseDTO
    {
        public string? CategoryType { get; set; }
        public string? CategoryName { get; set; }
        public bool? IsActive { get; set; }
    }
}
