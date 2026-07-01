using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SliceSync.Core.DTOs.Order;
using SliceSync.Core.Entities;
using SliceSync.Core.Enums;
using SliceSync.Core.ServiceContracts;
using SliceSync.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
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

            if (orderStatusRequestDTO.OrderStatusChangedTo == Core.Enums.OrderStatus.Cancelled.ToString())
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

        public async Task<OrderStatusUpdateResponseDTO> ReturnOrder(OrderStatusUpdateRequestDTO orderStatusRequestDTO)
        {
            //find user and order in DB
          var foundUser=await  _db.Orders.FirstOrDefaultAsync(o=>o.OrderId == orderStatusRequestDTO.OrderId && o.UserId==orderStatusRequestDTO.UserId);


            if (foundUser == null)
            {
                throw new KeyNotFoundException("User and its order Not found!!");
            }

            var previousOrderStatus=foundUser.OrderStatus;

            //Change order status to => Returned
            if (orderStatusRequestDTO.OrderStatusChangedTo == Core.Enums.OrderStatus.Returned.ToString())
            {
                foundUser.OrderStatus=Core.Enums.OrderStatus.Returned;
            }

             _db.Orders.Update(foundUser);
            await _db.SaveChangesAsync();

            //now add data in Order Status History table

            //find user role
            var userFound = await _db.UserRoles.FirstOrDefaultAsync(u => u.UserId == orderStatusRequestDTO.UserId);
            var userRoleId = userFound?.RoleId;

            var userRoleName = "";
            var DbRoleId = await _db.Roles.FirstOrDefaultAsync(a => a.Id == userFound.RoleId);
            if (DbRoleId != null)
            {
                userRoleName = DbRoleId.Name;
            }

            //Add current order status in OrderHistoryTable
            OrderStatusHistory orderStatusHistory = new OrderStatusHistory()
            {
                OrderStatusHistoryId = Guid.NewGuid(),
                OrderId = foundUser.OrderId,
                OrderStatus = foundUser.OrderStatus.ToString(),
                UserId = foundUser.UserId,
                Role = userRoleName,
                Note = "Order status changed to RETURN!",
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
                Message = "Order status changed to RETURN!"
            };
        }
    }
}
