using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SliceSync.Core.DTOs.Menu;
using SliceSync.Core.ServiceContracts;

namespace SliceSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class MenuController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;

        public MenuController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMenu()
        {
            var pizzas = (await _pizzaService.GetllAllPizzas())
                .Where(p => p.IsActive)
                .ToList();

            var menuItems = pizzas.Select(p => new MenuItemResponseDTO
            {
                Id = p.PizzaId,
                Name = p.PizzaName,
                UnitPrice = p.Unitprice,
                Ingredients = p.Categories?
                    .Where(c => !string.IsNullOrWhiteSpace(c.CategoryName))
                    .Select(c => c.CategoryName!)
                    .ToList() ?? new List<string>(),
                SoldOut = p.IsSoldOut,
                ImageUrl = p.Image
            }).ToList();

            return Ok(menuItems);
        }
    }
}
