using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoesSalePage.Models
{
<<<<<<< Updated upstream
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ShoesModel Shoes { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }

        public int OrderID { get; set; } // Foreign Key
=======
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartID { get; set; }
        public int Quantity { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }

        public virtual Order Order { get; set; }
    }
}