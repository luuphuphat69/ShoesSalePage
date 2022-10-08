using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoesSalePage.Models
{
    public class ShoesModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public string Brand { get; set; }
        [Display(Name = "Shoe Color")]
        public string Color { get; set; }
        public string Image { get; set; }
        [Display(Name ="Available")]
        [Required]
        public bool IsAvailable { get; set; }
    }
}