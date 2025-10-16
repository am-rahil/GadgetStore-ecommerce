using Ecommerce.Common.CommonDto;
using Ecommerce.Entity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Repository
{
    public interface ICartItemRepository
    {
        Task<Result<List<CartItemResponse>>> GetUserCart(int userId);
        Task<Result<CartItemResponse>> AddOrUpdateCartItem(CartItemRequest request);
        Task<Result<string>> RemoveCartItem(int cartId);
        Task<Result<string>> ClearCart(int userId);
    }
}
