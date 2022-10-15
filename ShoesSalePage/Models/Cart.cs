using System;
using System.ComponentModel.DataAnnotations;

namespace ShoesSalePage.Models
{
    public class Cart
    {
        public ShoesModel Shoes { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}