using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SliceSync.Core.DTOs;
using SliceSync.Core.DTOs.Cart;
using SliceSync.Core.ServiceContracts;

namespace SliceSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CustomerController : ControllerBase
    {

        private readonly IPizzaService _pizzaService;
        private readonly ICartService _cartService;

        public CustomerController(IPizzaService pizzaService, ICartService cartService)
        {
            _pizzaService = pizzaService;
            _cartService = cartService;
        }


        [HttpGet("pizzas")]
        public async Task<IActionResult> GetllPizza()
        {
           var allPizzas=await _pizzaService.GetllAllPizzas();

            return Ok(allPizzas);
        }

        [HttpPost("addtocart")]
        public async Task<ActionResult<AddToCartResponseDTO>> AddToCart(AddToCartRequestDTO requestDTO)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _cartService.AddToCart(requestDTO));
        }

        //[HttpPost("removefromcart")]
        //public async Task<ActionResult<AddToCartResponseDTO>> RemoveFromCart(AddToCartRequestDTO requestDTO)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    return Ok(await _cartService.RemoveFromCart(requestDTO));
        //}
    }
}
