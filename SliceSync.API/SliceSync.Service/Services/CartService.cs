using SliceSync.Core.DTOs.Cart;
using SliceSync.Core.Entities;
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
                    UserId = cartRequestDTO.UserId,
                    Quantity = 1
                };
            }

            CartItem? foundCartItem = _db.CartItem.FirstOrDefault(c => c.CartId == foundCart.CartId && c.PizzaId == foundPizza.PizzaId);

            //2. When cart is found but cartitem is not found
            if (foundCartItem == null)
            {
                CartItem newCartItem = new CartItem()
                {
                    CartItemId = Guid.NewGuid(),
                    CartId = foundCart.CartId,
                    PizzaId = foundPizza.PizzaId,
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
                    foundCart.IsActive = false;
                    _db.Carts.Update(foundCart);
                    _db.CartItem.Remove(foundCartItem);
                    await _db.SaveChangesAsync();

                    return new AddToCartResponseDTO()
                    {
                        PizzaId = foundPizza.PizzaId,
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
                UserId = foundCart.UserId,
                Quantity = foundCartItem.Quantity,
            };

            return response;
        }
    }
}
