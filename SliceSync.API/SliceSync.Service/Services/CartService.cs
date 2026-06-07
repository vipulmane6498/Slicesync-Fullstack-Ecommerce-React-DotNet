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

            // When cart is not found
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
                    Cart = newCart,
                    PizzaId = cartRequestDTO.PizzaId,
                    Quantity = 1,
                    PriceAtThatTime = foundPizza.Unitprice,
                    CartId = newCart.CartId,
                };

                //newCart.CartItems.Add(newCartItem);

                await _db.Carts.AddAsync(newCart);
                await _db.CartItem.AddAsync(newCartItem);
                await _db.SaveChangesAsync();

                return new AddToCartResponseDTO()
                {
                    PizzaId = foundPizza.PizzaId,
                    UserId = foundCart.UserId,
                    Quantity = 1,
                    TotalCartPrice=foundPizza.Unitprice
                };
            }

            CartItem? foundCartItem = _db.CartItem.FirstOrDefault(c => c.CartId == foundCart.CartId && c.PizzaId == foundPizza.PizzaId);

            // When cart is found but cartitem is not found
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
                foundCart.UpdatedAt = DateTime.UtcNow;

                await _db.CartItem.AddAsync(newCartItem);
                _db.Carts.Update(foundCart);
                await _db.SaveChangesAsync();

                return new AddToCartResponseDTO()
                {
                    PizzaId = foundPizza.PizzaId,
                    UserId = foundCart.UserId,
                    Quantity = 1,
                    TotalCartPrice = (decimal)foundCart.CartPrice
                };
            }

            // When cart and cartitem both are found
            foundCartItem.PriceAtThatTime = foundPizza?.Unitprice;
            foundCartItem.Quantity++;
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
                TotalCartPrice = (decimal)foundCart.CartPrice
            };
        }

        //public async Task<AddToCartResponseDTO> RemoveFromCart(AddToCartRequestDTO request)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
