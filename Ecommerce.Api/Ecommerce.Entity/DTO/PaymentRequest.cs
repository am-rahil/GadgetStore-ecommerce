using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entity.DTO
{
    public class PaymentRequest
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, StringLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal AmountPaid { get; set; }

        [StringLength(100)]
        public string TransactionId { get; set; }

        [StringLength(20)]
        public string PaymentStatus { get; set; } = "Pending";

    }
}
