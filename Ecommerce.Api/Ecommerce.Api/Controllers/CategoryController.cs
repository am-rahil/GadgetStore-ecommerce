using Ecommerce.Common.CommonDto;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        [HttpGet("GetAllCategories")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryRepository.GetAllCategories();
            if (result.isError)
                return NotFound(result.Errors);

            // Map to response DTO
            var response = result.Response.Select(c => new CategoryResponse
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description
            }).ToList();

            return Ok(new Result<List<CategoryResponse>> { Response = response });
        }



        [HttpGet("GetCategoryById")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryRepository.GetCategoryById(id);
            if (result.isError)
                return NotFound(result.Errors);

            var response = new CategoryResponse
            {
                CategoryId = result.Response.CategoryId,
                CategoryName = result.Response.CategoryName,
                Description = result.Response.Description
            };
            return Ok(new Result<CategoryResponse> { Response = response });
        }


        [HttpPost("AddCategory")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                CategoryName = request.CategoryName,
                Description = request.Description
            };

            await _categoryRepository.AddCategory(category);

            return Ok(new Result<string> { Response = "Category added successfully." });
        }



        [HttpPut("UpdateCategory")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequest request)
        {
            var existingCategory = await _categoryRepository.GetCategoryById(id);
            if (existingCategory.isError)
                return NotFound(existingCategory.Errors);

            var category = existingCategory.Response;
            category.CategoryName = request.CategoryName;
            category.Description = request.Description;

            await _categoryRepository.UpdateCategory(category);

            return Ok(new Result<string> { Response = "Category updated successfully." });
        }

        [HttpDelete("DeleteCategory")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _categoryRepository.GetCategoryById(id);
            if (existingCategory.isError)
                return NotFound(existingCategory.Errors);

            await _categoryRepository.DeleteCategory(id);
            return Ok(new Result<string> { Response = "Category deleted successfully." });
        }
    }
}
