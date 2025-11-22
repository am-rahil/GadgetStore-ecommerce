using Ecommerce.Entity.DTO;
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
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemController(  ICartItemRepository cartItemRepository1)
        {
            _cartItemRepository = cartItemRepository1;
        }


        [HttpGet("GetUserCart")]
        
        public async Task<IActionResult> GetUserCart(int userId)
        {
            var result = await _cartItemRepository.GetUserCart(userId);
            if (result.isError)
                return NotFound(result.Errors);

            return Ok(result.Response);
        }

        [HttpPost("AddOrUpdateCartItem")]
        
        public async Task<IActionResult> AddOrUpdateCartItem([FromBody] CartItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _cartItemRepository.AddOrUpdateCartItem(request);
            if (result.isError)
                return BadRequest(result.Errors);

            return Ok(result.Response);
        }

        [HttpDelete("ClearCart")]
        
        public async Task<IActionResult> ClearCart(int userId)
        {
            var result = await _cartItemRepository.ClearCart(userId);
            if (result.isError)
                return NotFound(result.Errors);

            return Ok(result.Response);
        }


        [HttpDelete("RemoveCartItem")]
        
        public async Task<IActionResult> RemoveCartItem(int cartId)
        {
            var result = await _cartItemRepository.RemoveCartItem(cartId);
            if (result.isError)
                return NotFound(result.Errors);

            return Ok(result.Response);
        }

    }
}
