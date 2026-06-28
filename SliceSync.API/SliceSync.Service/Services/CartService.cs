using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using SliceSync.Core.DTOs.Cart;
using SliceSync.Core.DTOs.Order;
using SliceSync.Core.DTOs.OrderItem;
using SliceSync.Core.Entities;
using SliceSync.Core.Enums;
using SliceSync.Core.ServiceContracts;
using SliceSync.Infrastructure.Data;

namespace SliceSync.Service.Services
{
    public class CartService : ICartService

    {
        private readonly AppDbContext _db;

        public CartService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<AddToCartResponseDTO> AddToCart(AddToCartRequestDTO cartRequestDTO)
        {

            Cart? foundCart = _db.Carts.FirstOrDefault(c => c.UserId == cartRequestDTO.UserId);

            Pizza foundPizza = _db.Pizzas.FirstOrDefault(p => p.PizzaId == cartRequestDTO.PizzaId) ?? throw new KeyNotFoundException("Invalid pizza id");

            //1. When cart is not found
            if (foundCart == null)
            {
                ////creating cart
                Cart newCart = new Cart()
                {
                    CartId = Guid.NewGuid(),
                    UserId = cartRequestDTO.UserId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    CartPrice = foundPizza.Unitprice,
                };

                CartItem newCartItem = new CartItem()
                {
                    CartItemId = Guid.NewGuid(),
                    CartId = newCart.CartId,
                    PizzaId = cartRequestDTO.PizzaId,
                    PizzaName = foundPizza.PizzaName,
                    Cart = newCart,
                    Quantity = 1,
                    PriceAtThatTime = foundPizza.Unitprice,
                };

                //newCart.CartItems.Add(newCartItem);

                await _db.Carts.AddAsync(newCart);
                await _db.CartItem.AddAsync(newCartItem);
                await _db.SaveChangesAsync();

                return new AddToCartResponseDTO()
                {
                    PizzaId = foundPizza.PizzaId,
                    PizzaName = foundPizza.PizzaName,
                    UserId = cartRequestDTO.UserId,
                    Quantity = 1
                };
            }

            //2. When cart is found but cartitem is not found
            CartItem? foundCartItem = _db.CartItem.FirstOrDefault(c => c.CartId == foundCart.CartId && c.PizzaId == foundPizza.PizzaId);

            if (foundCartItem == null)
            {
                CartItem newCartItem = new CartItem()
                {
                    CartItemId = Guid.NewGuid(),
                    CartId = foundCart.CartId,
                    PizzaId = foundPizza.PizzaId,
                    PizzaName = foundPizza.PizzaName,
                    Quantity = 1,
                    PriceAtThatTime = foundPizza.Unitprice,
                    Cart = foundCart,
                    Pizza = foundPizza,
                };

                foundCart.CartPrice += foundPizza?.Unitprice;
                foundCart.IsActive = true;
                foundCart.UpdatedAt = DateTime.UtcNow;

                await _db.CartItem.AddAsync(newCartItem);
                _db.Carts.Update(foundCart);
                await _db.SaveChangesAsync();

                return new AddToCartResponseDTO()
                {
                    PizzaId = foundPizza.PizzaId,
                    PizzaName = foundPizza.PizzaName,
                    UserId = foundCart.UserId,
                    Quantity = 1
                };
            }

            //3. When cart and cartitem both are found
            foundCartItem.PriceAtThatTime = foundPizza?.Unitprice;
            foundCartItem.Quantity++;
            foundCart.IsActive = true;
            foundCart.CartPrice += foundPizza?.Unitprice;
            foundCart.UpdatedAt = DateTime.UtcNow;

            _db.CartItem.Update(foundCartItem);
            _db.Carts.Update(foundCart);
            await _db.SaveChangesAsync();

            return new AddToCartResponseDTO()
            {
                PizzaId = foundPizza.PizzaId,
                PizzaName = foundPizza.PizzaName,
                UserId = foundCart.UserId,
                Quantity = foundCartItem.Quantity,

            };
        }


        public async Task<AddToCartResponseDTO> RemoveFromCart(AddToCartRequestDTO cartRequestDTO)
        {

            var foundCart = _db.Carts.FirstOrDefault(c => c.UserId == cartRequestDTO.UserId);
            var foundPizza = _db.Pizzas.FirstOrDefault(p => p.PizzaId == cartRequestDTO.PizzaId);

            //1. when cart is not found
            if (foundCart == null)
            {
                throw new KeyNotFoundException("Cart is not active for this user!");
            }

            //2. when cart is found but not cartItem
            var foundCartItem = _db.CartItem.FirstOrDefault(ci => ci.CartId == foundCart.CartId && ci.PizzaId == foundPizza.PizzaId);

            if (foundCartItem == null)
            {
                throw new KeyNotFoundException("Items not present in the cart!");
            }


            //3. when cart is found with cartItem
            if (foundCartItem != null)
            {
                foundCartItem.Quantity--;
                //when cart item reaches to zero
                foundCart.CartPrice -= foundPizza.Unitprice;
                foundCart.UpdatedAt = DateTime.UtcNow;

                if (foundCartItem.Quantity == 0)
                {
                    _db.CartItem.Remove(foundCartItem);
                    _db.Carts.Remove(foundCart);
                    foundCart.IsActive = false;
                    await _db.SaveChangesAsync();

                    return new AddToCartResponseDTO()
                    {
                        PizzaId = foundPizza.PizzaId,
                        PizzaName = foundPizza.PizzaName,
                        UserId = foundCart.UserId,
                        Quantity = foundCartItem.Quantity,
                    };
                }
            }

            _db.CartItem.Update(foundCartItem);
            _db.Carts.Update(foundCart);
            await _db.SaveChangesAsync();

            var response = new AddToCartResponseDTO()
            {
                PizzaId = foundPizza.PizzaId,
                PizzaName = foundPizza.PizzaName,
                UserId = foundCart.UserId,
                Quantity = foundCartItem.Quantity,
            };

            return response;
        }

        public async Task<OrderResponseDTO> CheckOut(OrderRequestDTO orderRequestDTO)
        {
            //1. find cart->cartItems->pizza and userid check if avail in db
            var foundCart = await _db.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Pizza).
                FirstOrDefaultAsync(c => c.UserId == orderRequestDTO.UserId
                                        && c.CartId == orderRequestDTO.CartId);

            if (foundCart == null)
            {
                throw new KeyNotFoundException("Cart not found!");
            }

            //2. now validate pizza in db
            var foundPizza = new List<Pizza>();
            foreach (var item in foundCart.CartItems)
            {
                var pizza = await _db.Pizzas.FirstOrDefaultAsync(p => p.PizzaId == item.PizzaId);
                if (pizza == null)
                {
                    throw new KeyNotFoundException($"Pizza with id {item.PizzaId} no longer exists!");
                }
                foundPizza.Add(pizza);
            }

            if (foundPizza == null)
            {
                throw new KeyNotFoundException("Pizza not exists");
            }

            //3. if foundCart and FoundPizza is exist then create order

            var orderCreated = new Order()
            {
                OrderId = Guid.NewGuid(),
                UserId = orderRequestDTO.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                OrderStatus = OrderStatus.OrderConfirmed,
                TotalOrderPrice = foundCart.CartPrice

            };
            var orderItem = foundCart.CartItems.Select(item => new OrderItem()
            {
                OrderItemId = Guid.NewGuid(),
                OrderId = orderCreated.OrderId,
                Order = orderCreated,
                PizzaId = item.PizzaId,
                PizzaName = item.PizzaName,
                Quantity = item.Quantity,
                PriceAtThatTime = foundPizza.FirstOrDefault(p => p.PizzaId == item.PizzaId).Unitprice
            }).ToList();


            await _db.Orders.AddAsync(orderCreated);
            await _db.OrderItem.AddRangeAsync(orderItem);
            await _db.SaveChangesAsync();

            //4. once the order placed erase the cart and cartItem
            _db.CartItem.RemoveRange(foundCart.CartItems);
            foundCart.IsActive = false;
            foundCart.CartPrice = 0;
            _db.Carts.Remove(foundCart);
            await _db.SaveChangesAsync();

            //Get User Role
            var foundUser = await _db.UserRoles.FirstOrDefaultAsync(u => u.UserId == orderRequestDTO.UserId);
            var UserRoleId = foundUser?.RoleId;

            var foundUserRoleId = await _db.Roles.FirstOrDefaultAsync(r => r.Id == UserRoleId);

            var UserRoleName = "";
            if (UserRoleId != null)
            {
                UserRoleName = foundUserRoleId?.Name;
            }

            //add placed order in OrderStatusHistory table
            OrderStatusHistory orderStatusHistory = new OrderStatusHistory()
            {
                OrderStatusHistoryId = Guid.NewGuid(),
                OrderId = orderCreated.OrderId,
                OrderStatus = orderCreated.OrderStatus.ToString(),
                UserId = orderCreated.UserId,
                Role = UserRoleName,
                Note = "Order Placed Successfully!",
                CreatedAt = DateTime.UtcNow,
            };
            await _db.OrderStatusHistories.AddAsync(orderStatusHistory);
            await _db.SaveChangesAsync();

            //return resonse to client
            var response = new OrderResponseDTO()
            {
                OrderId = orderCreated.OrderId,
                UserId = orderRequestDTO.UserId,
                OrderPlacedAt = DateTime.UtcNow,
                OrderStatus = orderCreated.OrderStatus,
                OrderPrice = orderCreated.TotalOrderPrice,
                OrderItems = orderItem.Select(oi => new OrderItemResonseDTO()
                {
                    PizzaId = oi.PizzaId,
                    PizzaName = oi.PizzaName,
                    Quantity = oi.Quantity,
                    ItemPrice = oi.PriceAtThatTime
                }).ToList()
            };
            return response;

        }
    }
}
