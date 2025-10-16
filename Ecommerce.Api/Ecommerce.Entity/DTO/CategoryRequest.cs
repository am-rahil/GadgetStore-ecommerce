using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entity.DTO
{
    public class CategoryRequest
    {
        [Required, MaxLength(100)]
        public string CategoryName { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
    }
}
