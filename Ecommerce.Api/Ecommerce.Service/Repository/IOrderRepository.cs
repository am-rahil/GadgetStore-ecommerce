using Ecommerce.Common.CommonDto;
using Ecommerce.Entity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ecommerce.Entity.DTO.OrderResponse;

namespace Ecommerce.Service.Repository
{
    public interface IOrderRepository   
    {
        Task<Result<List<OrderResponse>>> GetAllOrders();
        Task<Result<OrderResponse>> GetOrderById(int id);
        Task<Result<OrderResponse>> CreateOrder(OrderRequest request);
        Task<Result<string>> UpdateOrderStatus(int id, string status);
        Task<Result<string>> DeleteOrder(int id);
    }
}
