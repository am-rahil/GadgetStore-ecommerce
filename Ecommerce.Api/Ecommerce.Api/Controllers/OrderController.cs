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
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("GetAllOrders")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderRepository.GetAllOrders();
            if (result.isError)
                return NotFound(result.Errors);

            return Ok(result.Response);
        }

        [HttpGet("GetOrderById")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderRepository.GetOrderById(id);
            if (result.isError)
                return NotFound(result.Errors);

            return Ok(result.Response);
        }

        [HttpPost("CreateOrder")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderRepository.CreateOrder(request);
            if (result.isError)
                return BadRequest(result.Errors);

            return Ok(result.Response);
        }

        [HttpPut("UpdateOrderStatus")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromQuery] string status)
        {
            var result = await _orderRepository.UpdateOrderStatus(id, status);
            if (result.isError)
                return BadRequest(result.Errors);

            return Ok(result.Response);
        }

        [HttpDelete("DeleteOrder")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderRepository.DeleteOrder(id);
            if (result.isError)
                return NotFound(result.Errors);

            return Ok(result.Response);
        }

        [HttpGet("GetOrdersByUserId")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var result = await _orderRepository.GetAllOrders();
            if (result.isError)
                return NotFound(result.Errors);

            var userOrders = result.Response.Where(o => o.UserId == userId).ToList();
            return Ok(userOrders);
        }



    }
}
