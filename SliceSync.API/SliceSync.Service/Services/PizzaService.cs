using Azure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SliceSync.Core.DTOs;
using SliceSync.Core.Entities;
using SliceSync.Core.ServiceContracts;
using SliceSync.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Service.Services
{
    public class PizzaService : IPizzaService
    {
        private readonly AppDbContext _appDbContext;
        public PizzaService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<PizzaResponseDTO> AddPizza(PizzaRequestDTO pizzaDTO)
        {
            // 1. Fetch ALL requested categories from the DB in ONE single query
            var foundCategories = await _appDbContext.Categories
                .Where(c => pizzaDTO.CategoryId.Contains(c.CategoryId))
                .ToListAsync();

            if (!foundCategories.Any())
            {
                throw new Exception("None of the provided categories were found!!");
            }

            // 2. Map and Add Pizza
            var pizza = new Pizza()
            {
                PizzaId = Guid.NewGuid(),
                PizzaName = pizzaDTO.PizzaName,
                Unitprice = pizzaDTO.Unitprice,
                Image = pizzaDTO.ImageUrl,
                PizzaDesciption = pizzaDTO.PizzaDesciption,
                IsSoldOut = pizzaDTO.IsSoldOut,
                IsActive = pizzaDTO.IsActive,
                CreateAt = DateTime.Now
            };
            await _appDbContext.Pizzas.AddAsync(pizza);

            // 3. Map and Add multiple categories using AddRange (faster than a loop)
            var mappings = foundCategories.Select(category => new PizzaCategoryMapping
            {
                PizzaId = pizza.PizzaId,
                CategoryId = category.CategoryId
            }).ToList();

            await _appDbContext.PizzaCategoryMappings.AddRangeAsync(mappings);

            // 4. Commit everything to the database in one round-trip
            await _appDbContext.SaveChangesAsync();

            // 5. Build and return a clean Response DTO to the client
            var response = new PizzaResponseDTO
            {
                PizzaId = pizza.PizzaId,
                PizzaName = pizza.PizzaName,
                Unitprice = pizza.Unitprice,
                Image = pizza.Image,
                PizzaDesciption = pizza.PizzaDesciption,
                IsSoldOut = pizza.IsSoldOut ?? false,
                IsActive = pizza.IsActive ?? false,
                CreateAt = pizza.CreateAt ?? DateTime.UtcNow,
                Categories = foundCategories.Select(c => new CategoryResonseDTO
                {
                    CategoryType = c.CategoryType,
                    CategoryName = c.CategoryName, // Assuming property name is CategoryName
                    IsActive = c.IsActive
                }).ToList()
            };

            return response;
        }


        public async Task<PizzaResponseDTO> GetPizzaById(Guid id)
        {
            var foundPizza = await _appDbContext.Pizzas
                 .Include(p => p.PizzaCategoryMappings)
                 .ThenInclude(c => c.Category)
                 .FirstOrDefaultAsync(p => p.PizzaId == id);

            if (foundPizza == null)
            {
                throw new KeyNotFoundException($"Pizza id: {id} not found!");
            }

            var response = new PizzaResponseDTO()
            {
                PizzaId = foundPizza.PizzaId,
                PizzaName = foundPizza.PizzaName,
                Unitprice = foundPizza.Unitprice,
                Image = foundPizza.Image,
                PizzaDesciption = foundPizza.PizzaDesciption,
                IsSoldOut = foundPizza.IsSoldOut ?? false,
                IsActive = foundPizza.IsActive ?? false,
                CreateAt = foundPizza.CreateAt ?? DateTime.UtcNow,
                Categories = foundPizza.PizzaCategoryMappings?.Select(pcm => new CategoryResonseDTO
                {
                    CategoryType = pcm.Category.CategoryType,
                    CategoryName = pcm.Category.CategoryName,
                    IsActive = pcm.Category.IsActive ?? false,
                }).ToList() ?? new List<CategoryResonseDTO>()
            };

            return response;
        }

        public async Task<bool> DetelePizzaById(Guid id)
        {
            var foundPizza = await _appDbContext.Pizzas.Include(p => p.PizzaCategoryMappings)
                 .ThenInclude(pcm => pcm.Category)
                 .FirstOrDefaultAsync(p => p.PizzaId == id);

            if (foundPizza == null)
            {
                throw new KeyNotFoundException($"Provided pizza id: {id} not found!");
            }

            _appDbContext.Pizzas.Remove(foundPizza);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<PizzaResponseDTO>> GetllAllPizzas()
        {

            var allPizza = await _appDbContext.Pizzas.Include(p => p.PizzaCategoryMappings).ThenInclude(pcm => pcm.Category).ToListAsync();


            var responseList = new List<PizzaResponseDTO>();

            foreach (var pizza in allPizza)
            {
                var response = new PizzaResponseDTO()
                {
                    PizzaId = pizza.PizzaId,
                    PizzaName = pizza.PizzaName,
                    Unitprice = pizza.Unitprice,
                    Image = pizza.Image,
                    PizzaDesciption = pizza.PizzaDesciption,
                    IsSoldOut = pizza.IsSoldOut ?? false,
                    IsActive = pizza.IsActive ?? false,
                    CreateAt = pizza.CreateAt ?? DateTime.UtcNow,
                    Categories = pizza.PizzaCategoryMappings?.Select(pcm => new CategoryResonseDTO
                    {
                        CategoryType = pcm.Category.CategoryType,
                        CategoryName = pcm.Category.CategoryName,
                        IsActive = pcm.Category.IsActive ?? false,
                    }).ToList() ?? new List<CategoryResonseDTO>()
                };
                responseList.Add(response);
            }

            return responseList;
        }

        public async Task<bool> DeleteAllPizzas()
        {
            List<Pizza> getAllPizzas = await _appDbContext.Pizzas.ToListAsync();

            _appDbContext.Pizzas.RemoveRange(getAllPizzas);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<PizzaResponseDTO> UpdatePizza(Guid id, PizzaRequestDTO pizzaRequestDTO)
        {
            //Check if pizza id found
            var foundPizza = await _appDbContext.Pizzas.Include(p => p.PizzaCategoryMappings)
                 .ThenInclude(c => c.Category)
                 .FirstOrDefaultAsync(p => p.PizzaId == id);

            if (foundPizza == null)
            {
                throw new KeyNotFoundException($"Provided Pizza id: {id} not available!");
            }

            //check if category id found
            List<Category> foundCategories = new List<Category>();
            foreach (var cat in pizzaRequestDTO.CategoryId)
            {
                var result = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == cat);

                if (result != null)
                {
                    foundCategories.Add(result);

                }
            }
            if (foundCategories == null)
            {
                throw new KeyNotFoundException($"Provided Category id not available!");
            }

            //update pizza
            foundPizza.PizzaName = pizzaRequestDTO.PizzaName;
            foundPizza.Unitprice = pizzaRequestDTO.Unitprice;
            foundPizza.Image = pizzaRequestDTO.ImageUrl;
            foundPizza.PizzaDesciption = pizzaRequestDTO.PizzaDesciption;
            foundPizza.IsSoldOut = pizzaRequestDTO.IsSoldOut;
            foundPizza.IsActive = pizzaRequestDTO.IsActive;

            // Remove all existing category mappings for this pizza
            // This ensures no old categories remain before adding the updated ones
            foundPizza.PizzaCategoryMappings.Clear();

            //updating pizzaCategoryMapping=> adding categoryid one by one received from pizzaRequestResponse
            foreach (var category in foundCategories)
            {
                foundPizza.PizzaCategoryMappings.Add(new PizzaCategoryMapping()
                {
                    PizzaId = id,
                    CategoryId = category.CategoryId
                });
            }
            var updatedPizza = _appDbContext.Pizzas.Update(foundPizza);
            await _appDbContext.SaveChangesAsync();

            //return the responseDTO
            var response = new PizzaResponseDTO()
            {
                PizzaId = id,
                PizzaName = foundPizza.PizzaName,
                Unitprice = foundPizza.Unitprice,
                Image = foundPizza.Image,
                PizzaDesciption = foundPizza.PizzaDesciption,
                IsSoldOut = foundPizza.IsSoldOut ?? false,
                IsActive = foundPizza.IsActive ?? true,
                CreateAt = foundPizza.CreateAt ?? DateTime.UtcNow,

                Categories = foundPizza.PizzaCategoryMappings?.Select(pcm => new CategoryResonseDTO
                {
                    CategoryType = pcm.Category.CategoryType,
                    CategoryName = pcm.Category.CategoryName,
                    IsActive = pcm.Category.IsActive
                }).ToList() ?? new List<CategoryResonseDTO>()
            };

            return response;
        }
    }

}
