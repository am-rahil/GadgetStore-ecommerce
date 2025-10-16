using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entity.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required, MaxLength(150)]
        public string SupplierName { get; set; }

        [MaxLength(200)]
        public string ContactEmail { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        // Relationship
        public ICollection<Product> Products { get; set; }
    }
}
