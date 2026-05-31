using SliceSync.Core.DTOs;
using SliceSync.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.ServiceContracts
{
    public interface ICategoryService
    {
        
        public Task<Category> AddCategory(CategoryResonseDTO categoryDTO);

        public Task<Category> UpdateCategory(CategoryResonseDTO categoryDTO);

        public Task<bool> DeleteCategoryById(Guid id);
         public Task<bool> DeleteAllCategories();

        public Task<List<Category>> GetAllCategories();

        public Task<Category> GetCategoryById(Guid id);

        public Task<List<Category>> GetCategoryByType(string categoryType);




    }
}
