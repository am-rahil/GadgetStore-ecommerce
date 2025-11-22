using Ecommerce.Common.CommonDto;
using Ecommerce.Entity.DTO;
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
        Task<Result<List<ProductResponse>>> GetProductsByCategory(int categoryId);
        Task<Result<ProductResponse>> GetProductById(int id);

        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
        Task DeleteAllGalleryImages(int productId);
        Task AddProductImage(ProductImage image);
        Task<Product?> GetProductEntityById(int id);
    }
}
