using System;
using System.ComponentModel.DataAnnotations;

namespace ShoesSalePage.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public ShoesModel Shoes { get; set; }
    }
}