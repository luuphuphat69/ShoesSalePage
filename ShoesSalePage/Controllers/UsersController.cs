using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ShoesSalePage.Models;

namespace ShoesSalePage.Controllers
{
    public class UsersController : Controller
    {
        private ShoesSalePageEnityEntities db = new ShoesSalePageEnityEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View();
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
                var check = db.Users.FirstOrDefault(s => s.UserName == user.UserName);
                if (check == null)
                {
                    user.Password = GetMD5(user.Password);
                    db.Configuration.ValidateOnSaveEnabled = false; // Disable entity validation
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    ViewBag.UserMessage = "User name is used. Try another name";
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
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
                    Session["UserName"] = data.UserName;
                    Session["UserEmail"] = data.Email;
                    Session["UserCreatedDate"] = data.CreatedDate;
                    Session["UserPhoneNumber"] = data.PhoneNumber;
                    Session["UserCity"] = data.City;
                    Session["UserAddress"] = data.Address;
                    Session["UserPostalCode"] = data.PostalCode;
                    Session["UserCreatedDate"] = data.CreatedDate;
                }
                return RedirectToAction("Index");
            }
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
    }
}
