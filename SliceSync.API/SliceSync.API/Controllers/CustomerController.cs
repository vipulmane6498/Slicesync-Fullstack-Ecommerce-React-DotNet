using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SliceSync.Core.DTOs;
using SliceSync.Core.DTOs.Cart;
using SliceSync.Core.DTOs.Order;
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
        private readonly IOrderStatusService _orderStatusService;

        public CustomerController(IPizzaService pizzaService, ICartService cartService, IOrderStatusService orderStatusService)
        {
            _pizzaService = pizzaService;
            _cartService = cartService;
            _orderStatusService = orderStatusService;
        }


        [HttpGet("pizzas")]
        public async Task<IActionResult> GetllPizza()
        {
            var allPizzas = await _pizzaService.GetllAllPizzas();

            return Ok(allPizzas);
        }

        [HttpPost("addtocart")]
        public async Task<ActionResult<AddToCartResponseDTO>> AddToCart(AddToCartRequestDTO requestDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _cartService.AddToCart(requestDTO));
        }

        [HttpPost("removefromcart")]
        public async Task<ActionResult<AddToCartResponseDTO>> RemoveFromCart(AddToCartRequestDTO requestDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _cartService.RemoveFromCart(requestDTO));
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<OrderResponseDTO>> CheckOut(OrderRequestDTO orderRequestDTO)
        {
            var OrderPlaced = await _cartService.CheckOut(orderRequestDTO);

            return Ok(OrderPlaced);
        }


        //cancel order => Happens before the order is shipped/delivered
        [HttpPatch("cancelorder")]
        public async Task<ActionResult<OrderStatusUpdateResponseDTO>> CancelOrder(OrderStatusUpdateRequestDTO orderStatusUpdateRequestDTO)
        {
            if (orderStatusUpdateRequestDTO == null)
            {
                return BadRequest(ModelState);
            }
            var CancelledOrder = await _orderStatusService.CancelOrder(orderStatusUpdateRequestDTO);
            return Ok(CancelledOrder);

        }

        //Return order => Happens after the order is delivered
        [HttpPatch("returnorder")]
        public async Task<ActionResult<OrderStatusUpdateResponseDTO>> ReturnOrder(OrderStatusUpdateRequestDTO orderStatusUpdateRequestDTO)
        {
            if (orderStatusUpdateRequestDTO == null)
            {
                return BadRequest(ModelState);
            }
            var ReturnedOrder = await _orderStatusService.ReturnOrder(orderStatusUpdateRequestDTO);
            return Ok(ReturnedOrder);

        }
    }
}
    