using SliceSync.Core.DTOs.Cart;
using SliceSync.Core.DTOs.Order;
using SliceSync.Core.DTOs.OrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.ServiceContracts
{
    public interface ICartService
    {
        public Task<AddToCartResponseDTO> AddToCart(AddToCartRequestDTO cartRequestDTO);
        Task<AddToCartResponseDTO> RemoveFromCart(AddToCartRequestDTO request);

        public Task<OrderResponseDTO> CheckOut(OrderRequestDTO orderRequestDTO);
    }
}
