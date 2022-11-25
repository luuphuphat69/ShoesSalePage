using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesSalePage.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Brand { get; set; }
        public string Image { get; set; }
        public string Size { get; set; }
        public bool IsAvailable { get; set; }
        public string Color { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Quantity { get; set; }
        public virtual Stock Stock { get; set; }
    }
}