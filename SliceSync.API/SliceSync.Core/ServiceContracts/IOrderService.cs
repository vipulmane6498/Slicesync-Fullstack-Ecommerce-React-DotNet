using SliceSync.Core.DTOs.Order;

namespace SliceSync.Core.ServiceContracts
{
    public interface IOrderService
    {
        Task<FrontendOrderResponseDTO> CreateOrder(CreateOrderFromCartRequestDTO requestDTO, Guid? userId = null);
        Task<FrontendOrderResponseDTO> GetOrderById(Guid orderId);
        Task<FrontendOrderResponseDTO> UpdateOrderPriority(Guid orderId);
        Task<List<FrontendOrderResponseDTO>> GetOrdersByUserId(Guid userId);
    }
}
