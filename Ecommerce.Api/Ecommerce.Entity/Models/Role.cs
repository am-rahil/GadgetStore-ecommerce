using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entity.Models
{
    public class Role: IdentityRole<int>
    
    {
        [Required, MaxLength(50)]
        public string DisplayName { get; set; } // Optional readable name

        public ICollection<ApplicationUser> Users { get; set; }

    }
}
