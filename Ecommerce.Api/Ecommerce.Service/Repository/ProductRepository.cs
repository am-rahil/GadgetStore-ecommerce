using Ecommerce.Common.CommonDto;
using Ecommerce.Entity.DTO;
using Ecommerce.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddProduct(Product product)
        {
            await _context.ProductsSet.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.ProductsSet.FindAsync(id);
            if (product != null)
            {
                _context.ProductsSet.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Result<List<Product>>> GetAllProducts()
        {
            Result<List<Product>> result = new();

            var products = await _context.ProductsSet
                                         .Include(p => p.Category)
                                         .Include(p => p.Supplier)
                                         .ToListAsync();

            if (products.Any())
                result.Response = products;
            else
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "No products found" });

            return result;
        }

        public async Task<Result<ProductResponse>> GetProductById(int id)
        {
            var result = new Result<ProductResponse>();

            var product = await _context.ProductsSet
                                        .Include(p => p.Category)
                                        .Include(p => p.Supplier)
                                        .Include(p => p.productImages)
                                        .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "Product not Found" });
                return result;
            }

            var response = new ProductResponse
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImagePath = product.ImagePath,   // old single image
                ImagePaths = product.productImages
                     .Select(pi => pi.ImagePath)
                     .ToList(),

                CategoryId = product.Category?.CategoryId,
                CategoryName = product.Category?.CategoryName,
                SupplierId = product.Supplier?.SupplierId,
                SupplierName = product.Supplier?.SupplierName
            };
            result.Response = response;
            return result;
        }

        public async Task<Result<List<ProductResponse>>> GetProductsByCategory(int categoryId)
        {
            var result = new Result<List<ProductResponse>>();

            var products = await _context.ProductsSet
                .AsNoTracking()
                .Include(p => p.Category)   // safe to include then project
                .Include(p => p.Supplier)
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new ProductResponse
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    ImagePath = p.ImagePath,
                    CategoryId = p.Category != null ? p.Category.CategoryId : (int?)null,
                    CategoryName = p.Category != null ? p.Category.CategoryName : null,
                    SupplierId = p.Supplier != null ? p.Supplier.SupplierId : (int?)null,
                    SupplierName = p.Supplier != null ? p.Supplier.SupplierName : null
                })
                .ToListAsync();

            if (products.Any())
                result.Response = products;
            else
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "No products found for this category" });

            return result;
        }


        public async Task UpdateProduct(Product product)
        {
            _context.ProductsSet.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task AddProductImage(ProductImage image)
        {
            await _context.productImages.AddAsync(image);
            await _context.SaveChangesAsync();
        }


        public async Task<Product?> GetProductEntityById(int id)
        {
            return await _context.ProductsSet
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task DeleteAllGalleryImages(int productId)
        {
            var images = await _context.productImages
                .Where(pi => pi.ProductId == productId)
                .ToListAsync();

            if (images.Count == 0)
                return;

            // Delete image files
            foreach (var img in images)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.ImagePath);
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            _context.productImages.RemoveRange(images);
            await _context.SaveChangesAsync();
        }



    }
}
