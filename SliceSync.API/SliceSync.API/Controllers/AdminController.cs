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
            var categories = await _categoryService.AddCategories(categoryDTO);

            return Ok(categories);
            
        }

        
        [HttpPut("editcategory")]
        public async Task<IActionResult> EditCategory(CategoryDTO categoryDTO)
        {
           var updatedCategory= await _categoryService.UpdateCategories(categoryDTO);

            return Ok(updatedCategory);
        }

    }
}
