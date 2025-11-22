using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entity.Models
{
    public class ProductImage
    {
        [Key]
        public int ProductImageId { get; set; }

        [Required, MaxLength(500)]
        public string ImagePath { get; set; }

        // Foreign key
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
