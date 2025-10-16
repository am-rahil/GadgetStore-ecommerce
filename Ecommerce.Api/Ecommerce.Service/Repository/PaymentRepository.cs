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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        //add payment
        public async Task AddPayment(Payment payment)
        {
            _context.PaymentsSet.Add(payment);
            await _context.SaveChangesAsync();
        }


        //update payment
        public async Task UpdatePayment(Payment payment)
        {
            _context.PaymentsSet.Update(payment);
            await _context.SaveChangesAsync();

        }



        //delete payment
        public async Task DeletePayment(int id)
        {
            var payment = await _context.PaymentsSet.FindAsync(id);
            if (payment != null)
            {
                _context.PaymentsSet.Remove(payment);
                await _context.SaveChangesAsync();

            }
        }

        //Get payment by id
        public async Task<Result<Payment>> GetPaymentById(int id)
        {
            var result = new Result<Payment>();
            var payment = await _context.PaymentsSet
                .Include(p => p.User)
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
            if (payment != null)
            {
                result.Response = payment;
            }
            else
            {
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "Payment not found" });
            }
            return result;
        }

        //get all payments
        public async Task<Result<List<Payment>>> GetAllPayments()
        {
            var result = new Result<List<Payment>>();
            var payments = await _context.PaymentsSet
                .Include(p => p.User)
                .Include(p => p.Order)
                .ToListAsync();

            if (payments.Count > 0)
            {
                result.Response = payments;
            }
            else
            {
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "No payments found" });
            }
            return result;  
        }



    }
}
