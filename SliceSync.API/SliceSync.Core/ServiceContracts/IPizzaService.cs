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

        public Task<PizzaResponseDTO> UpdatePizza(Guid id, PizzaRequestDTO pizzaRequestDTO);

        public Task<PizzaResponseDTO> GetPizzaById(Guid id);

        public Task<List<PizzaResponseDTO>> GetllAllPizzas();

        public Task<bool> DetelePizzaById(Guid id);

        public Task<bool> DeleteAllPizzas();
    }
}
