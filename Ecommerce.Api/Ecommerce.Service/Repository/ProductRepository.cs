using Ecommerce.Common.CommonDto;
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

        public async Task<Result<Product>> GetProductById(int id)
        {
            Result<Product> result = new();

            var product = await _context.ProductsSet
                                        .Include(p => p.Category)
                                        .Include(p => p.Supplier)
                                        .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product != null)
                result.Response = product;
            else
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "Product not found" });

            return result;
        }

        public async Task UpdateProduct(Product product)
        {
            _context.ProductsSet.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
