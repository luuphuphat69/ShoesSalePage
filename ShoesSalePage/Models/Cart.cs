using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoesSalePage.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ShoesModel Shoes { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }

        public int OrderID { get; set; } // Foreign Key
        public virtual Order Order { get; set; }
    }
}