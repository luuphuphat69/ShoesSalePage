﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ShoesSalePage.Models
{
<<<<<<< Updated upstream
    public class User
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // specifies that the value will only be generated by the
                                                              // database when a value is first added to the database.
        public int UserId { get; set; } 
        
        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        [StringLength(100, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name="Tên tài khoản (viết liền - không dấu)")]
        public string UserName {set; get;}

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [NotMapped] // not create corresponding columns in the database
        [Required(ErrorMessage = "Không được để trống phần này")]
        [Compare("Password", ErrorMessage = "Mật khẩu không giống nhau")]
        [Display(Name = "Nhập lại mật khẩu")]
        public string ConfirmPass { get; set; }

        [Required(ErrorMessage = "Không được để trống phần này")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Họ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Không được để trống phần này")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Tên")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Mail không được để trống")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", // Regex
         ErrorMessage = "Email không phù hợp")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(50)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Thành phố không được để trống")]
        [StringLength(50)]
        [Display(Name = "Thành phố")]
=======
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Orders = new HashSet<Order>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        [Required]
        [Compare("Password", ErrorMessage = "Password not match")]
        public string ConfirmPass { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
>>>>>>> Stashed changes
        public string City { get; set; }
        [Display(Name = "Mã bưu điện")]
        public string PostalCode { get; set; }
<<<<<<< Updated upstream
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(11)]
        [Display(Name = "Số điện thoại")]
=======
        [Required]
>>>>>>> Stashed changes
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.Now;
    }
}