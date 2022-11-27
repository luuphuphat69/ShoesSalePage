//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShoesSalePage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Stocks = new HashSet<Stock>();
        }

        [Key]
        [Required(ErrorMessage = "This filed can not be null")]
        [Display(Name = "ID")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "This filed can not be null")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This filed can not be null")]
        [Display(Name = "Product Price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "This filed can not be null")]
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "This filed can not be null")]
        public bool IsAvailable { get; set; }
        [Required(ErrorMessage = "This filed can not be null")]
        [Display(Name = "Category ID")]
        public Nullable<int> CategoryId { get; set; }
        [Required(ErrorMessage = "This filed can not be null")]
        [Display(Name = "Subcategory ID")]
        public Nullable<int> SubCategoryId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
