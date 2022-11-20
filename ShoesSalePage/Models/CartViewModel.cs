using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesSalePage.Models
{
    public class CartViewModel
    {
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

    }
}