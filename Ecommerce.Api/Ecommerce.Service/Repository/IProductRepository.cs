using Ecommerce.Common.CommonDto;
using Ecommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Repository
{
    public interface IProductRepository
    {
        Task<Result<List<Product>>> GetAllProducts();
        Task<Result<Product>> GetProductById(int id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
    }
}
