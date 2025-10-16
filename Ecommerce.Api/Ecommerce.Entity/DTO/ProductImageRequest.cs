using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entity.DTO
{
    public   class ProductImageRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required, MaxLength(255)]
        public string ImageUrl { get; set; }
    }
}
