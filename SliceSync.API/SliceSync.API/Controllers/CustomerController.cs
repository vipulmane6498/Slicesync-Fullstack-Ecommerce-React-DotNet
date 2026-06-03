using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SliceSync.Core.DTOs;
using SliceSync.Core.ServiceContracts;

namespace SliceSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Customer")]
    public class CustomerController : ControllerBase
    {

        private readonly IPizzaService _pizzaService;

        public CustomerController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }


        [HttpGet("pizzas")]
        public async Task<IActionResult> GetllPizza()
        {
           var allPizzas=await _pizzaService.GetllAllPizzas();

            return Ok(allPizzas);
        }
    }
}
