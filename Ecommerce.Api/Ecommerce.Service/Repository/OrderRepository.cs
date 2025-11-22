using Azure.Core;
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
                address=request.address,
                pincode=request.pincode,
                Phone=request.Phone,
                OrderDetails = request.OrderDetails.Select(d => new OrderDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    ImagePath = d.ImagePath,
                    SubTotal = d.Quantity * d.UnitPrice
                }).ToList()
            };

            _context.OrdersSet.Add(order);
            await _context.SaveChangesAsync();

            //  create payment record
            var payment = new Payment
            {
                OrderId = order.OrderId,
                UserId = request.UserId,
                PaymentMethod = request.Status,
                AmountPaid = request.TotalAmount,
                PaymentStatus = request.Status == "COD" ? "Pending" : "Paid",
                PaymentDate = DateTime.Now
            };
            await _context.PaymentsSet.AddAsync(payment);
            await _context.SaveChangesAsync();

            var userCart = _context.CartsSet.Where(c => c.UserId == request.UserId);
            _context.CartsSet.RemoveRange(userCart);
            await _context.SaveChangesAsync();

            //  Reduce stock when order is placed
            foreach (var detail in order.OrderDetails)
            {
                var product = await _context.ProductsSet.FindAsync(detail.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= detail.Quantity;
                    if (product.StockQuantity < 0)
                        product.StockQuantity = 0;
                }
            }
            await _context.SaveChangesAsync();

            result.Response = new OrderResponse
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                address = request.address,
                pincode = request.pincode,
                Phone = request.Phone,
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailResponse
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    ImagePath = d.ImagePath,
                    

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
                address = o.address,
                pincode = o.pincode,
                Phone = o.Phone,
                OrderDetails = o.OrderDetails.Select(d => new OrderDetailResponse
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product?.ProductName,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    ImagePath = d.Product?.ImagePath
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
                address = order.address,
                pincode = order.pincode,
                Phone = order.Phone,
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailResponse
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product?.ProductName,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    ImagePath = d.Product?.ImagePath
                }).ToList()
            };

            return result;
        }

        public async Task<Result<string>> UpdateOrderStatus(int id, string status)
        {
            Result<string> result = new();
            var order = await _context.OrdersSet
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "Order not found" });
                return result;
            }

            var validStatuses = new List<string> { "Pending", "Shipped", "Delivered", "Cancelled" };

            if (!validStatuses.Contains(status))
            {
                result.Errors.Add(new Errors { ErrorCode = "400", ErrorMessage = "Invalid status value" });
                return result;
            }

            if (order.Status == "Delivered" || order.Status == "Cancelled")
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "400",
                    ErrorMessage = "Cannot update a completed or cancelled order"
                });
                return result;
            }

            //  If order is cancelled → increase stock back
            if (status == "Cancelled")
            {
                foreach (var detail in order.OrderDetails)
                {
                    var product = await _context.ProductsSet.FindAsync(detail.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity += detail.Quantity; // restore stock
                    }
                }
            }

            //  When marking as delivered → reduce product stock
            if (status == "Delivered")
            {
                foreach (var detail in order.OrderDetails)
                {
                    var product = await _context.ProductsSet.FindAsync(detail.ProductId);
                    if (product != null && product.StockQuantity < 0)
                    {
                        product.StockQuantity = 0;
                    }
                }
            }


            // Update order status
            order.Status = status;
            _context.OrdersSet.Update(order);
            await _context.SaveChangesAsync();

            result.Response = $"Order status updated to {status} successfully.";
            return result;
        }





    }
}
