using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entity.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        [MaxLength(50)]
        public string PaymentMethod { get; set; }  // e.g., CreditCard, UPI, etc.

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "Pending";

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }
    }
}

