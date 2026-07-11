using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SliceSync.Core.DTOs.Order;
using SliceSync.Core.ServiceContracts;
using System.Security.Claims;

namespace SliceSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderFromCartRequestDTO requestDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Extract user ID from JWT if the request is authenticated
            Guid? userId = null;
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                userIdClaim = User.FindFirstValue("sub");
            }

            if (Guid.TryParse(userIdClaim, out var parsedId))
                userId = parsedId;

            if (userId is null)
                return Unauthorized();

            var order = await _orderService.CreateOrder(requestDTO, userId);
            return Ok(order);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id)
        {
            var order = await _orderService.GetOrderById(id);
            return Ok(order);
        }

        [HttpPatch("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, [FromBody] UpdateOrderPriorityRequestDTO requestDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.UpdateOrderPriority(id);
            return Ok(order);
        }

        [HttpGet("mine")]
        [Authorize]
        public async Task<IActionResult> GetMyOrders()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var orders = await _orderService.GetOrdersByUserId(userId);
            return Ok(orders);
        }
    }
}
