using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entity.DTO
{

    public class OrderResponse
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string address { get; set; }
        public string pincode { get; set; }
        public string Phone { get; set; }

        public List<OrderDetailResponse> OrderDetails { get; set; }
    }

    public class OrderDetailResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => Quantity * UnitPrice;
        public string ImagePath { get; set; }
        


    }

}
