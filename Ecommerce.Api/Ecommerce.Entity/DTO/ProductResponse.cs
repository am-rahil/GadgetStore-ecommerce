using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entity.DTO
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImagePath { get; set; }
        public List<string> ImagePaths { get; set; } = new();

        // Category
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        // Supplier
        public int? SupplierId { get; set; }
        public string? SupplierName { get; set; }
    }
}
