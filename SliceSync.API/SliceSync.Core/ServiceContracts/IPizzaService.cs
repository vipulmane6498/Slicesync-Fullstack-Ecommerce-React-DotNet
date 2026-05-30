using SliceSync.Core.DTOs;
using SliceSync.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.ServiceContracts
{
    public interface IPizzaService
    {

        public Task<PizzaResponseDTO> AddPizza(PizzaRequestDTO pizzaDTO);
    }
}
