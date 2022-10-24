namespace ShoesSalePage.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ShoesDbTable")]
    public partial class ShoesModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public double Price { get; set; }

        public int Size { get; set; }

        [Required]
        public string Brand { get; set; }

        public string Color { get; set; }

        public string Image { get; set; }

        public bool IsAvailable { get; set; }
    }
}
