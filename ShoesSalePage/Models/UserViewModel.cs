using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesSalePage.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public string UserAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}