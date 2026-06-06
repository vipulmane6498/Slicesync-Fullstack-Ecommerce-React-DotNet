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
        
        public Task<CategoryResponseDTO> AddCategory(CategoryRequestDTO categoryRequestDTO);

        public Task<CategoryResponseDTO> UpdateCategory(CategoryRequestDTO categoryRequestDTO);

        public Task<bool> DeleteCategoryById(Guid id);
         public Task<bool> DeleteAllCategories();

        public Task<List<CategoryResponseDTO>> GetAllCategories();
        
        public Task<CategoryResponseDTO> GetCategoryById(Guid id);

        public Task<List<Category>> GetCategoryByType(string categoryType);




    }
}
