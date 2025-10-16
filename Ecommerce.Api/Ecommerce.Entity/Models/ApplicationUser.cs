using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entity.Models
{
    public class ApplicationUser : IdentityUser <int> // using int as primary key

    {
        [Required, MaxLength(100)]
        public string FullName { get; set; }

        

        // Relationships
        public ICollection<Order> Orders { get; set; }
        public ICollection<CartItem> Carts { get; set; }
        public ICollection<Payment> Payments { get; set; }


    }
}
