using Ecommerce.Common.CommonDto;
using Ecommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Repository
{
    public interface IPaymentRepository
    {
        Task<Result<List<Payment>>> GetAllPayments();
        Task<Result<Payment>> GetPaymentById(int id);
        Task AddPayment(Payment payment);
        Task UpdatePayment(Payment payment);
        Task DeletePayment(int id);


    }
}
