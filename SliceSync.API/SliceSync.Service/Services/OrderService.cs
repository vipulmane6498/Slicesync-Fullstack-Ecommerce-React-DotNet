using Microsoft.EntityFrameworkCore;
using SliceSync.Core.DTOs.Order;
using SliceSync.Core.Entities;
using SliceSync.Core.Enums;
using SliceSync.Core.ServiceContracts;
using SliceSync.Infrastructure.Data;

namespace SliceSync.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _db;

        public OrderService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<FrontendOrderResponseDTO> CreateOrder(CreateOrderFromCartRequestDTO requestDTO, Guid? userId = null)
        {
            if (requestDTO.Cart == null || !requestDTO.Cart.Any())
                throw new ArgumentException("Cart cannot be empty.");

            // Validate all pizzas exist and get authoritative prices from DB
            var pizzaIds = requestDTO.Cart.Select(c => c.PizzaId).ToList();
            var pizzas = await _db.Pizzas
                .Where(p => pizzaIds.Contains(p.PizzaId))
                .ToListAsync();

            if (pizzas.Count != pizzaIds.Distinct().Count())
                throw new KeyNotFoundException("One or more pizza IDs in the cart are invalid.");

            var pizzaLookup = pizzas.ToDictionary(p => p.PizzaId);

            // Build order items using DB-validated prices
            var orderItems = requestDTO.Cart.Select(cartItem =>
            {
                var pizza = pizzaLookup[cartItem.PizzaId];
                return new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    PizzaId = cartItem.PizzaId,
                    PizzaName = pizza.PizzaName,
                    Quantity = cartItem.Quantity,
                    PriceAtThatTime = pizza.Unitprice
                };
            }).ToList();

            // Calculate prices
            decimal orderPrice = orderItems.Sum(oi => oi.Quantity * (oi.PriceAtThatTime ?? 0));
            decimal priorityPrice = requestDTO.Priority ? Math.Round(orderPrice * 0.2m, 2) : 0m;

            // Estimated delivery: 30 min for priority, 45 min for normal
            var estimatedDelivery = DateTime.UtcNow.AddMinutes(requestDTO.Priority ? 30 : 45);

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                OrderStatus = OrderStatus.OrderConfirmed,
                Priority = requestDTO.Priority,
                PriorityPrice = priorityPrice,
                TotalOrderPrice = orderPrice,
                EstimatedDelivery = estimatedDelivery,
                CustomerName = requestDTO.Customer,
                CustomerPhone = requestDTO.Phone,
                CustomerAddress = requestDTO.Address,
                Position = requestDTO.Position
            };

            foreach (var item in orderItems)
            {
                item.OrderId = order.OrderId;
                item.Order = order;
            }

            await _db.Orders.AddAsync(order);
            await _db.OrderItem.AddRangeAsync(orderItems);
            await _db.SaveChangesAsync();

            return MapToFrontendDTO(order, orderItems);
        }

        public async Task<FrontendOrderResponseDTO> GetOrderById(Guid orderId)
        {
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                throw new KeyNotFoundException($"Order #{orderId} not found.");

            return MapToFrontendDTO(order, order.OrderItems ?? new List<OrderItem>());
        }

        public async Task<FrontendOrderResponseDTO> UpdateOrderPriority(Guid orderId)
        {
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                throw new KeyNotFoundException($"Order #{orderId} not found.");

            if (!order.Priority)
            {
                order.Priority = true;
                order.PriorityPrice = Math.Round((order.TotalOrderPrice ?? 0) * 0.2m, 2);
                order.UpdatedAt = DateTime.UtcNow;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
            }

            return MapToFrontendDTO(order, order.OrderItems ?? new List<OrderItem>());
        }

        public async Task<List<FrontendOrderResponseDTO>> GetOrdersByUserId(Guid userId)
        {
            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders
                .Select(o => MapToFrontendDTO(o, o.OrderItems ?? new List<OrderItem>()))
                .ToList();
        }

        private static FrontendOrderResponseDTO MapToFrontendDTO(Order order, IEnumerable<OrderItem> orderItems)
        {
            return new FrontendOrderResponseDTO
            {
                Id = order.OrderId,
                Status = order.OrderStatus?.ToString() ?? "OrderConfirmed",
                Priority = order.Priority,
                PriorityPrice = order.PriorityPrice ?? 0m,
                OrderPrice = order.TotalOrderPrice ?? 0m,
                EstimatedDelivery = order.EstimatedDelivery ?? DateTime.UtcNow.AddMinutes(45),
                CreatedAt = order.CreatedAt,
                CustomerName = order.CustomerName,
                CustomerAddress = order.CustomerAddress,
                Cart = orderItems.Select(oi => new FrontendCartItemDTO
                {
                    PizzaId = oi.PizzaId,
                    Name = oi.PizzaName,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.PriceAtThatTime ?? 0m,
                    TotalPrice = oi.Quantity * (oi.PriceAtThatTime ?? 0m)
                }).ToList()
            };
        }
    }
}
