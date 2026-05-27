using Microsoft.AspNetCore.Mvc.Formatters;
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
    public class CategoryService : ICategoryService
    {

        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context) {
            _context = context;
        }

       

        public async Task<Category> AddCategories(CategoryDTO dto)
        {

            var category = new Category()
            {
                CategoryId = Guid.NewGuid(),
                CategoryType = dto.CategoryType,
                CategoryName = dto.CategoryName,
                IsActive = dto.IsActive

                //var pizzaCategorymapping = new PizzaCategoryMapping()
                //{
                //    CategoryId = category.CategoryId,
                //    PizzaId = 3
                //}
            };

            var createdCategories = await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();

            return createdCategories.Entity;
            
        }
    }
}
