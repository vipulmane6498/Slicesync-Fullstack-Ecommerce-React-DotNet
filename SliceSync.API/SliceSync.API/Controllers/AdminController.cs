using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SliceSync.Core.DTOs;
using SliceSync.Core.Entities;
using SliceSync.Core.ServiceContracts;

namespace SliceSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public AdminController(ICategoryService categoryService) {
        
            _categoryService = categoryService;
        }

        [HttpPost("addcategory")]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO)
        {
            var categories = await _categoryService.AddCategory(categoryDTO);

            return Ok(categories);
            
        }

        
        [HttpPut("editcategory")]
        public async Task<IActionResult> EditCategory(CategoryDTO categoryDTO)
        {
           var updatedCategory= await _categoryService.UpdateCategory(categoryDTO);

            return Ok(updatedCategory);
        }

        [HttpDelete("removecategorybyid")]
        public async Task<IActionResult> RemoveCategoryById(Guid id)
        {
          var deletedCategory= await _categoryService.DeleteCategoryById(id);

        return Ok($"Provided id: {id} is deleted !!");
        }

    }
}
