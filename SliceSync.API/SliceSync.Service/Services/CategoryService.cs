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

       

        public async Task<CategoryResponseDTO> AddCategory(CategoryRequestDTO categoryRequestDTO)
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


            var response = new CategoryResponseDTO()
            {
                CategoryId=category.CategoryId,
                CategoryType = category.CategoryType,
                CategoryName = category.CategoryName,
                IsActive = category.IsActive
            };

            return response;
            
        }


        public async Task<CategoryResponseDTO> UpdateCategory(CategoryRequestDTO categoryRequestDTO)
        {
            //find in db
            Category? category = await _context.Categories.FirstOrDefaultAsync(a => a.CategoryType == categoryRequestDTO.CategoryType);

            if (category == null)
            {
                throw new Exception("Category should not be null here !");
            }

            //update in Category           
            category.CategoryName = categoryRequestDTO.CategoryName;
            category.IsActive = categoryRequestDTO.IsActive;

            _context.Update(category);
            await _context.SaveChangesAsync();

            var response = new CategoryResponseDTO()
            {
                CategoryId=category.CategoryId,
                CategoryType = category.CategoryType,
                CategoryName = category.CategoryName,
                IsActive = category.IsActive
            };

            return response;
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

        public async Task<List<CategoryResponseDTO>> GetAllCategories()
        {
            //check in db 
           var foundAllcategories= await _context.Categories.ToListAsync();

            //if categories not avail return execption
            if(foundAllcategories == null)
            {
                throw new KeyNotFoundException("Categegories not available!");
            }

            //if categories avail in db then return to clientwith following steps


            //create response list where in we will save each category 
            List<CategoryResponseDTO> foundCatList = new List<CategoryResponseDTO>();

            //traverse each category and found in DB and add in response
            foreach(var category in foundAllcategories)
            {
                //create obj of DTO wherein we will add category one by one
                var response = new CategoryResponseDTO()
                {
                    CategoryId = category.CategoryId,
                    CategoryType = category.CategoryType,
                    CategoryName = category.CategoryName,
                    IsActive = category.IsActive
                };
                //eventually add in ResponseList
                foundCatList.Add(response);
            };

            return foundCatList;
        }

        public async Task<CategoryResponseDTO> GetCategoryById(Guid id)
        {
          Category? category=  await  _context.Categories.FirstOrDefaultAsync(a => a.CategoryId == id);

            if(category == null)
            {
                throw new Exception("Please provide some id! ");
            }

            var response = new CategoryResponseDTO()
            {
                CategoryId = category.CategoryId,
                CategoryType = category.CategoryType,
                CategoryName = category.CategoryName,
                IsActive = category.IsActive
            };
            return response;
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
