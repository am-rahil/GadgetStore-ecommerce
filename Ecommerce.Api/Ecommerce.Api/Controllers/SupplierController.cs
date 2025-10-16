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
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpGet("GetAllSuppliers")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var result = await _supplierRepository.GetAllSuppliers();
            if (result.isError)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("GetSupplierById")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var result = await _supplierRepository.GetSupplierById(id);
            if (result.isError)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost("AddSupplier")]
        public async Task<IActionResult> AddSupplier([FromBody] SupplierRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplier = new Supplier
            {
                SupplierName = request.SupplierName,
                ContactEmail = request.ContactEmail,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address
            };

            await _supplierRepository.AddSupplier(supplier);
            return Ok(new { message = "Supplier added successfully" });
        }

        [HttpPut("UpdateSupplier")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierRequest request)
        {
            var existing = await _supplierRepository.GetSupplierById(id);
            if (existing.isError)
                return NotFound(existing);

            var supplier = existing.Response;
            supplier.SupplierName = request.SupplierName;
            supplier.ContactEmail = request.ContactEmail;
            supplier.PhoneNumber = request.PhoneNumber;
            supplier.Address = request.Address;

            await _supplierRepository.UpdateSupplier(supplier);
            return Ok(new { message = "Supplier updated successfully" });
        }

        [HttpDelete("DeleteSupplier")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            await _supplierRepository.DeleteSupplier(id);
            return Ok(new { message = "Supplier deleted successfully" });
        }
    }
}
