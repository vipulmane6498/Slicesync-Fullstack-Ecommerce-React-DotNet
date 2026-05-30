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
                Categories = foundCategories.Select(c => new CategoryDTO
                {
                    CategoryType=c.CategoryType,
                    CategoryName = c.CategoryName, // Assuming property name is CategoryName
                   IsActive=c.IsActive
                }).ToList()
            };

            return response;



        }
    }
}
