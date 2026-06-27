using Microsoft.EntityFrameworkCore;
using SliceSync.Core.DTOs.Order;
using SliceSync.Core.Entities;
using SliceSync.Core.ServiceContracts;
using SliceSync.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Service.Services
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly AppDbContext _db;

        public OrderStatusService(AppDbContext appDbContext)
        {
            _db = appDbContext;
        }

        public async Task<OrderStatusUpdateResponseDTO> CancelOrder(OrderStatusUpdateRequestDTO orderStatusRequestDTO)
        {
            var foundUser = _db.Orders.FirstOrDefault(o => o.OrderId == orderStatusRequestDTO.OrderId && o.UserId == orderStatusRequestDTO.UserId);


            if (foundUser == null)
            {
                throw new KeyNotFoundException("Data Not found!!");
            }
            var previousOrderStatus = foundUser?.OrderStatus;

            if (orderStatusRequestDTO.OrderStatusChangedTo == "Cancelled")
            {
                foundUser.OrderStatus = Core.Enums.OrderStatus.Cancelled;
            }

            _db.Orders.Update(foundUser);
            await _db.SaveChangesAsync();

            //find user role
            var userFound = await _db.UserRoles.FirstOrDefaultAsync(u => u.UserId == orderStatusRequestDTO.UserId);
            var userRoleId = userFound.RoleId;

            var userRoleName = "";
            var DbRoleId = await _db.Roles.FirstOrDefaultAsync(a => a.Id == userFound.RoleId);
            if (DbRoleId != null)
            {
                userRoleName = DbRoleId.Name;
            }

            //add the current order status in OrderStatusHistory tabe
            OrderStatusHistory orderStatusHistory = new OrderStatusHistory()
            {
                OrderStatusHistoryId = Guid.NewGuid(),
                OrderId = foundUser.OrderId,
                OrderStatus = foundUser.OrderStatus.ToString(),
                UserId = foundUser.UserId,
                Role = userRoleName,
                Note = "Order mistekenly placed hence cancelled!",
                CreatedAt = DateTime.UtcNow,
            };
            await _db.OrderStatusHistories.AddAsync(orderStatusHistory);
            await _db.SaveChangesAsync();

            //return response
            return new OrderStatusUpdateResponseDTO()
            {
                OrderId = orderStatusRequestDTO.OrderId,
                UserId = orderStatusRequestDTO.UserId,
                OrderChangedAt = DateTime.UtcNow,
                PreviousOrderStatus = previousOrderStatus,
                CurrentOrderStatus = foundUser.OrderStatus,
                Message = "Order mistekenly placed hence cancelled!"
            };
        }

        public Task<OrderStatusUpdateResponseDTO> UpdateOrderStatus(OrderStatusUpdateRequestDTO orderStatusRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
