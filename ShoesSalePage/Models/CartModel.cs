using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesSalePage.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ShoesModel Shoes { get; set; }
    }
}