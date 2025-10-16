using Ecommerce.Common.CommonDto;
using Ecommerce.Entity.DTO;
using Ecommerce.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ecommerce.Entity.DTO.OrderResponse;

namespace Ecommerce.Service.Repository
{
    public class OrderRepository:IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<OrderResponse>> CreateOrder(OrderRequest request)
        {
            Result<OrderResponse> result = new();

            var order = new Order
            {
                UserId = request.UserId,
                TotalAmount = request.TotalAmount,
                Status = "Pending",
                OrderDetails = request.OrderDetails.Select(d => new OrderDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };

            _context.OrdersSet.Add(order);
            await _context.SaveChangesAsync();

            result.Response = new OrderResponse
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailResponse
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };

            return result;
        }

        public async Task<Result<string>> DeleteOrder(int id)
        {
            Result<string> result = new();
            var order = await _context.OrdersSet.FindAsync(id);

            if (order == null)
            {
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "Order not found" });
                return result;
            }

            _context.OrdersSet.Remove(order);
            await _context.SaveChangesAsync();

            result.Response = "Order deleted successfully";
            return result;
        }

        public async Task<Result<List<OrderResponse>>> GetAllOrders()
        {
            Result<List<OrderResponse>> result = new();
            var orders = await _context.OrdersSet
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.User)
                .ToListAsync();

            if (!orders.Any())
            {
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "No orders found" });
                return result;
            }

            result.Response = orders.Select(o => new OrderResponse
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                UserName = o.User?.UserName,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                OrderDetails = o.OrderDetails.Select(d => new OrderDetailResponse
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product?.ProductName,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            }).ToList();

            return result;
        }

        public async Task<Result<OrderResponse>> GetOrderById(int id)
        {
            Result<OrderResponse> result = new();
            var order = await _context.OrdersSet
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "Order not found" });
                return result;
            }

            result.Response = new OrderResponse
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                UserName = order.User?.UserName,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailResponse
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product?.ProductName,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };

            return result;
        }

        public async Task<Result<string>> UpdateOrderStatus(int id, string status)
        {
            Result<string> result = new();
            var order = await _context.OrdersSet.FindAsync(id);

            if (order == null)
            {
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "Order not found" });
                return result;
            }

            order.Status = status;
            _context.OrdersSet.Update(order);
            await _context.SaveChangesAsync();

            result.Response = "Order status updated successfully";
            return result;
        }
    }
}
