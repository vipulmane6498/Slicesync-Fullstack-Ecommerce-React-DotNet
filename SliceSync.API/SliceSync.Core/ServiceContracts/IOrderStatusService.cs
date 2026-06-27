using SliceSync.Core.DTOs.Order;
using SliceSync.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.ServiceContracts
{
    public interface IOrderStatusService
    {
        //Customer
        public Task<OrderStatusUpdateResponseDTO> CancelOrder(OrderStatusUpdateRequestDTO orderStatusRequestDTO);

        //Admin
        public Task<OrderStatusUpdateResponseDTO> UpdateOrderStatus(OrderStatusUpdateRequestDTO orderStatusRequestDTO);
    }
}
