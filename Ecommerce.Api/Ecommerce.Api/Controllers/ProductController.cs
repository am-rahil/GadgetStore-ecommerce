using Ecommerce.Entity.DTO;
using Ecommerce.Entity.Models;
using Ecommerce.Service.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

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
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productRepository.GetAllProducts();
            if (result.isError)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("GetProductById")]
        [AllowAnonymous]

        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productRepository.GetProductById(id);
            if (result.isError)
                return NotFound(result);
            return Ok(result);
        }


        [HttpPost("AddProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] ProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? mainImagePath = null;

            // ⭐ 1. SAVE MAIN IMAGE
            if (request.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(request.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                mainImagePath = "Images/" + fileName;
            }

            // ⭐ 2. CREATE PRODUCT
            var product = new Product
            {
                ProductName = request.ProductName,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId,
                SupplierId = request.SupplierId,
                ImagePath = mainImagePath
            };

            await _productRepository.AddProduct(product);

            // ⭐ 3. SAVE MULTIPLE GALLERY IMAGES
            if (request.GalleryImages != null && request.GalleryImages.Count > 0)
            {
                var savedImages = new List<ProductImage>();

                foreach (var img in request.GalleryImages)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    savedImages.Add(new ProductImage
                    {
                        ProductId = product.ProductId,
                        ImagePath = "ProductImages/" + fileName
                    });
                }

                // Save all images
                foreach (var image in savedImages)
                {
                    await _productRepository.AddProductImage(image);
                }
            }

            return Ok(new
            {
                message = "Product created successfully with all images",
                productId = product.ProductId
            });
        }


        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepository.GetProductEntityById(id);
            if (product == null)
                return NotFound("Product not found.");

            product.ProductName = request.ProductName;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.CategoryId = request.CategoryId;
            product.SupplierId = request.SupplierId;

            // ⭐ UPDATE MAIN IMAGE
            if (request.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(request.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImagePath);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                product.ImagePath = "Images/" + uniqueFileName;
            }

            // ⭐ ADD NEW GALLERY IMAGES
            if (request.GalleryImages != null && request.GalleryImages.Count > 0)
            {
                foreach (var img in request.GalleryImages)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    var newImage = new ProductImage
                    {
                        ProductId = product.ProductId,
                        ImagePath = "ProductImages/" + fileName
                    };

                    await _productRepository.AddProductImage(newImage);
                }
            }

            await _productRepository.UpdateProduct(product);

            return Ok(new
            {
                message = "Product updated successfully (including additional images)",
                product
            });
        }

        [HttpDelete("DeleteProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
            return Ok(new { message = "Product deleted successfully" });
        }

        [HttpGet("GetProductsByCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var result = await _productRepository.GetProductsByCategory(categoryId);
            if (result.isError)
                return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("ClearGalleryImages")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ClearGalleryImages(int productId)
        {
            await _productRepository.DeleteAllGalleryImages(productId);
            return Ok(new { message = "All gallery images deleted successfully!" });
        }



    }
}
