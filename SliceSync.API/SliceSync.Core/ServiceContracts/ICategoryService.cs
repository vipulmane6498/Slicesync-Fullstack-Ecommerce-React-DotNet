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
        
        public Task<Category> AddCategories(CategoryDTO categoryDTO);

        public Task<Category> UpdateCategories(CategoryDTO categoryDTO);


    }
}
