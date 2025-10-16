using Ecommerce.Entity.DTO;
using Ecommerce.Entity.Models;
using Ecommerce.Service.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productRepository.GetAllProducts();
            if (result.isError)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("GetProductById")]

        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productRepository.GetProductById(id);
            if (result.isError)
                return NotFound(result);
            return Ok(result);
        }


        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] ProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            //Image upload
            string? imagePath = null;

            if (request.ImageFile != null)
            {
                // Define a folder path for images 
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                // store relative path (for easy access in frontend)
                imagePath = $"Images/{uniqueFileName}";
            }

            var product = new Product
            {
                ProductName = request.ProductName,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId,
                SupplierId = request.SupplierId,
                ImagePath=imagePath
            };

            await _productRepository.AddProduct(product);
            return Ok(new { message = "Product added successfully",
            
            });
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequest request)
        {
            var existing = await _productRepository.GetProductById(id);
            if (existing.isError)
                return NotFound(existing);

            var product = existing.Response;
            product.ProductName = request.ProductName;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.CategoryId = request.CategoryId;
            product.SupplierId = request.SupplierId;

            await _productRepository.UpdateProduct(product);
            return Ok(new { message = "Product updated successfully" });
        }


        
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
            return Ok(new { message = "Product deleted successfully" });
        }

    }
}
