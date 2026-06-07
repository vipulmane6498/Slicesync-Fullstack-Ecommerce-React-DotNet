using SliceSync.Core.DTOs.Cart;
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
    }
}
