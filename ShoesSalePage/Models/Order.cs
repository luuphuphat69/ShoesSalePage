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
<<<<<<< HEAD
<<<<<<< Updated upstream
    public class Order
=======
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Order
>>>>>>> Test
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Carts = new HashSet<Cart>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
<<<<<<< HEAD
        public int ID { get; set; }
        public List<Cart> Cart { get; set; }
=======
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Carts = new HashSet<Cart>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
=======
>>>>>>> Test
        public int OrderID { get; set; }
        public int UserID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual User User { get; set; }
<<<<<<< HEAD
>>>>>>> Stashed changes
=======
>>>>>>> Test
    }
}
