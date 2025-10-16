using Azure.Core;
using Ecommerce.Common.CommonDto;
using Ecommerce.Entity.DTO;
using Ecommerce.Entity.Models;
using Ecommerce.Service.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        // GET: api/<PaymentController>


        [HttpGet("GetallPayments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var result = await _paymentRepository.GetAllPayments();
            if (result.isError)
                return BadRequest(result.Errors);

            return Ok(result.Response);
        }


        [HttpGet("GetPaymentById")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var result = await _paymentRepository.GetPaymentById(id);

            if (result.isError)
                return NotFound(result.Errors);

            return Ok(result.Response);
        }


        [HttpPost("AddPayment")]
        public async Task<IActionResult> AddPayment([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var payment = new Payment
            {
                OrderId = request.OrderId,
                UserId = request.UserId,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = request.AmountPaid,
                PaymentStatus = request.PaymentStatus,
                PaymentDate = DateTime.Now
            };

            await _paymentRepository.AddPayment(payment);

            var response = new PaymentResponse
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                UserId = payment.UserId,
                PaymentMethod = payment.PaymentMethod,
                AmountPaid = payment.AmountPaid,
                PaymentStatus = payment.PaymentStatus,
                PaymentDate = payment.PaymentDate
            };

            var result = new Result<PaymentResponse> { Response = response };
            return Ok(result);

        }


        [HttpPut("Update Payment")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentRequest request)
        {
            var existingPayment = await _paymentRepository.GetPaymentById(id);
            if (existingPayment.isError || existingPayment.Response == null)
                return NotFound(existingPayment.Errors);

            var payment = existingPayment.Response;
            payment.PaymentMethod = request.PaymentMethod;
            payment.AmountPaid = request.AmountPaid;
            payment.PaymentStatus = request.PaymentStatus;

            await _paymentRepository.UpdatePayment(payment);

            return Ok(new Result<string> { Response = "Payment updated successfully." });
        }

        // DELETE 
        [HttpDelete("Delete Payment")]
        public async Task<IActionResult> DeletePayment(int id)
        {

            var existingPayment = await _paymentRepository.GetPaymentById(id);
            if (existingPayment.isError)
                return NotFound(existingPayment.Errors);

            await _paymentRepository.DeletePayment(id);
            return Ok(new Result<string> { Response = "Payment deleted successfully." });
        }
    }
}
