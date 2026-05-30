using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SliceSync.Core.Entities
{
    public class PizzaCategoryMapping
    {
        [JsonIgnore]
        public Pizza? Pizza { get; set; }


        [JsonIgnore]
        public Category? Category { get; set; }


        //Foreign Key
        public Guid PizzaId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
