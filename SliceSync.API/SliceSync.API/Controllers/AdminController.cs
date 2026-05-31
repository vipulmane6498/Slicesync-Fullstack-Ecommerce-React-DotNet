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
        private readonly IPizzaService _pizzaService;

        public AdminController(ICategoryService categoryService, IPizzaService pizzaService) {
        
            _categoryService = categoryService;
            _pizzaService= pizzaService;    
        }

        [HttpPost("addcategory")]
        public async Task<IActionResult> AddCategory(CategoryResonseDTO categoryDTO)
        {
            var categories = await _categoryService.AddCategory(categoryDTO);

            return Ok(categories);
            
        }

        
        [HttpPut("editcategory")]
        public async Task<IActionResult> EditCategory(CategoryResonseDTO categoryDTO)
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


        [HttpGet("getallcategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            List<Category> allCategories = await _categoryService.GetAllCategories();

            if (allCategories != null)
            {
                return Ok(allCategories);
            }

            return NotFound("Categories does not exist, Please add !!");
        }


        [HttpGet("getcategorybyid")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
          var category= await _categoryService.GetCategoryById(id);

            return Ok(category);
        }

        [HttpGet("getcategorybytype")]
        public async Task<IActionResult> GetCategoryByType([FromQuery]string categoryType)
        {

            if(categoryType == null)
            {
                return Problem("Please provide the CategoryType!!");
            }

           var receivedCategory= await _categoryService.GetCategoryByType(categoryType);

            return Ok(receivedCategory);
        }

        [HttpDelete("removeallcategories")]
        public async Task<IActionResult> RemoveAllCategories()
        {
            var deletedAllCategories =await  _categoryService.DeleteAllCategories();
            return Ok("Deleted All Categories");
        }

        //---------------Pizza----------------------------

        [HttpPost("addpizza")]
        public async Task<IActionResult> AddPizza(PizzaRequestDTO pizza)
        {
            if (pizza == null)
            {
                return BadRequest("Please provide Pizza Details to add !");
            }

            var pizzaAdded = await _pizzaService.AddPizza(pizza);

            return Ok(pizzaAdded);
        }

        [HttpGet("getpizzabyid")]
        public async Task<IActionResult> GetPizzaById([FromQuery]Guid id)
        {
            if(id == null)
            {
                return BadRequest("Please provide id !!");
            }

            var pizza = await _pizzaService.GetPizzaById(id);

            return Ok(pizza);
        }

    }
}
