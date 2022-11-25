using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShoesSalePage.Models;

namespace ShoesSalePage.Controllers
{
    public class UsersController : Controller
    {
        private ShoesSalePageEnityEntities db = new ShoesSalePageEnityEntities();

        // GET: Users
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Users");
            int id = (int)Session["UserID"];
            var user = db.Users.FirstOrDefault(p => p.UserId == id);
            return View(user);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.UserName == user.UserName || s.PhoneNumber == user.PhoneNumber || s.Email == user.Email);
                if (check == null)
                {
                    user.Password = GetMD5(user.Password);
                    db.Configuration.ValidateOnSaveEnabled = false; // Disable entity validation
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    ViewBag.UserNameError = "User name is used or invalid. Try another name";
                    ViewBag.UserPhoneNumberError = "Phonenumber is used or invalid. Try another phonenumber";
                    ViewBag.UserEmailError = "Email is used or invalid. Try another email";
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
                return RedirectToAction("Index", "Users");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                var _password = GetMD5(password);
                var data = db.Users.FirstOrDefault(s => s.UserName.Equals(userName) && s.Password.Equals(_password));
                if (data == null)
                {
                    return View();
                }
                else
                {
                    Session["UserID"] = data.UserId;
                    Session["UserFullName"] = data.FirstName + data.LastName;
                    Session["UserFirstName"] = data.FirstName;
                    Session["UserLastName"] = data.LastName;
                    Session["UserName"] = data.UserName;
                    Session["UserEmail"] = data.Email;
                    Session["UserCreatedDate"] = data.CreatedDate;
                    Session["UserPhoneNumber"] = data.PhoneNumber;
                    Session["UserCity"] = data.City;
                    Session["UserAddress"] = data.Address;
                    Session["UserLocation"] = data.Address + data.City;
                    Session["UserCreatedDate"] = data.CreatedDate;
                }
                return RedirectToAction("Index");
            }
            if (Session["UserID"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        public static string GetMD5(string str) // mã hóa str thành chuỗi dữ liệu 128 bit 
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult OrderHistory(int? userId, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 4;
            if (Session["UserID"] != null)
            {
                var orderHistory = from s in db.Carts
                                   join k in db.Products on s.ProductID
                                   equals k.ProductId
                                   orderby s.OrderId
                                   where userId == s.Order.UserId
                                   select new CartViewModel
                                   {
                                       OrderId = s.OrderId,
                                       ProductId = s.ProductID,
                                       ProductName = k.Name,
                                       Size = s.Size,
                                       Brand = k.Brand,
                                       Price = k.Price,
                                       Quantity = s.Quantity,
                                       CreatedDate = s.CreatedDate
                                   };
                return View(orderHistory.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "User");
        }
    }
}
