using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesSalePage.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}