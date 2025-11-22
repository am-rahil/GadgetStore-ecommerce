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
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<CartItemResponse>> AddOrUpdateCartItem(CartItemRequest request)
        {
            Result<CartItemResponse> result = new();

            var existingItem = await _context.CartsSet
                .FirstOrDefaultAsync(c => c.UserId == request.UserId && c.ProductId == request.ProductId);

            var product = await _context.ProductsSet.FindAsync(request.ProductId);
            if (product == null)
            {
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "Product not found" });
                return result;
            }

            if (existingItem != null)
            {
                existingItem.Quantity = request.Quantity;
                existingItem.TotalPrice = existingItem.Quantity * product.Price;
                _context.CartsSet.Update(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    UserId = request.UserId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    TotalPrice = request.Quantity * product.Price
                };
                await _context.CartsSet.AddAsync(newItem);
            }

            await _context.SaveChangesAsync();

            result.Response = new CartItemResponse
            {
                ProductId = request.ProductId,
                ProductName = product.ProductName,
                Quantity = request.Quantity,
                TotalPrice = request.Quantity * product.Price
            };

            return result;
        }

        public async Task<Result<string>> ClearCart(int userId)
        {
            Result<string> result = new();

            var userCart = _context.CartsSet.Where(c => c.UserId == userId);
            if (!userCart.Any())
            {
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "No items found in cart" });
                return result;
            }

            _context.CartsSet.RemoveRange(userCart);
            await _context.SaveChangesAsync();

            result.Response = "Cart cleared successfully";
            return result;
        }

        public async Task<Result<List<CartItemResponse>>> GetUserCart(int userId)
        {
            Result<List<CartItemResponse>> result = new();

            var cartItems = await _context.CartsSet
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .Select(c => new CartItemResponse
                {
                    CartId = c.CartId,
                    UserId = c.UserId,
                    ProductId = c.ProductId,
                    ProductName = c.Product.ProductName,
                    Quantity = c.Quantity,
                    TotalPrice = c.TotalPrice,
                    ProductPrice = c.Product.Price,
                    ProductImagePath = c.Product.ImagePath,
                    CategoryName = c.Product.Category.CategoryName,
                    SupplierName = c.Product.Supplier.SupplierName,
                    Description=c.Product.Description

                }).ToListAsync();

            if (cartItems.Any())
                result.Response = cartItems;
            else
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "No items found in cart" });

            return result;
        }

        public async Task<Result<string>> RemoveCartItem(int cartId)
        {
            Result<string> result = new();
            var cartItem = await _context.CartsSet.FindAsync(cartId);

            if (cartItem == null)
            {
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "Cart item not found" });
                return result;
            }

            _context.CartsSet.Remove(cartItem);
            await _context.SaveChangesAsync();

            result.Response = "Item removed successfully";
            return result;
        }
    }
}
