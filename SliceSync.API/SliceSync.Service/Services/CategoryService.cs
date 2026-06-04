using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Formatters;
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
    public class CategoryService : ICategoryService
    {

        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context) {
            _context = context;
        }

       

        public async Task<CategoryResonseDTO> AddCategory(CategoryRequestDTO categoryRequestDTO)
        {

            var category = new Category()
            {
                CategoryId = Guid.NewGuid(),
                CategoryType = categoryRequestDTO.CategoryType,
                CategoryName = categoryRequestDTO.CategoryName,
                IsActive = categoryRequestDTO.IsActive

                //var pizzaCategorymapping = new PizzaCategoryMapping()
                //{
                //    CategoryId = category.CategoryId,
                //    PizzaId = 3
                //}
            };

            var createdCategories = await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();


            var response = new CategoryResonseDTO()
            {
                CategoryType = category.CategoryType,
                CategoryName = category.CategoryName,
                IsActive = category.IsActive
            };

            return response;
            
        }


        public async Task<Category> UpdateCategory(CategoryResonseDTO categoryDTO)
        {
            //find in db
            Category? category = await _context.Categories.FirstOrDefaultAsync(a => a.CategoryType == categoryDTO.CategoryType);

            if (category == null)
            {
                throw new Exception("Category should not be null here !");
            }

            //update in Category           
            category.CategoryName = categoryDTO.CategoryName;
            category.IsActive = categoryDTO.IsActive;

            _context.Update(category);
            await _context.SaveChangesAsync();

            return category;
        }
        public async Task<bool> DeleteCategoryById(Guid id)
        {
          Category? category= await _context.Categories.FirstOrDefaultAsync(a => a.CategoryId == id);

            if(category == null)
            {                
            throw new Exception("Please provide some id! ");
            }


            _context.Remove(category);

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<List<Category>> GetAllCategories()
        {
           return await _context.Categories.ToListAsync();            
        }

        public async Task<Category> GetCategoryById(Guid id)
        {
          Category? category=  await  _context.Categories.FirstOrDefaultAsync(a => a.CategoryId == id);

            if(category == null)
            {
                throw new Exception("Please provide some id! ");
            }
            return category;
        }


        public async Task<bool> DeleteAllCategories()
        {

            //Get all categories from DB
            List<Category> getAllCategories =await _context.Categories.ToListAsync();

            //delete from categories from db
            _context.RemoveRange(getAllCategories);

            await _context.SaveChangesAsync();

            return true;


        }

        public async Task<List<Category>> GetCategoryByType(string categoryType)
        {
            //find  the type in db and fetch all
            List<Category> category = await _context.Categories.Where(a => a.CategoryType == categoryType).ToListAsync();

            //if(category == null)
            //{
            //    throw new Exception("Please provide the existing category type!!");
            //}
            // //return it
            return category;
        }
    }
}
