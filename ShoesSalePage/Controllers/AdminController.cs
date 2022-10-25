using ShoesSalePage.Models;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace ShoesSalePage.Controllers
{
    public class AdminController : Controller
    {
        private readonly ShoesSalePageEntities db = new ShoesSalePageEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["AdminID"] != null)
                return View();
            else
                return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Admin admin)
        {
            if (ModelState.IsValid)
            {
                var check = db.Admins.FirstOrDefault(s => s.PhoneNumber == admin.PhoneNumber);
                if(check == null)
                {
                    admin.AdminPassword = GetMD5(admin.AdminPassword);
                    db.Configuration.ValidateOnSaveEnabled = false; // Disable entity validation
                    db.Admins.Add(admin);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.AdminMessage = "Số điện thoại đã được sử dụng";
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
        public ActionResult Login(string AdminName, string password)
        {
            if(ModelState.IsValid)
            {
                var _password = GetMD5(password);
                var data = db.Admins.Where(s => s.AdminName.Equals(AdminName) && s.AdminPassword.Equals(_password)).ToList();
                if (data != null)
                {
                    Session["AdminID"] = data.FirstOrDefault().AdminID;
                    Session["FullName"] = data.FirstOrDefault().FullName;
                    Session["AdminName"] = data.FirstOrDefault().AdminName;
                    Session["PhoneNumber"] = data.FirstOrDefault().PhoneNumber;
                    return RedirectToAction("Index");
                }
                else
                    ViewBag.Error = "Login failed. Try again";
                    return View();
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Admin");
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
        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            var check = db.Products.FirstOrDefault(s => s.ProductId == product.ProductId);
            if (check == null)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            else
                ViewBag.AddError = "Sản phẩm đã tồn tại";
            return View();
        }
        public ActionResult RemoveProduct(int productId)
        {
            var check = db.Products.FirstOrDefault(s => s.ProductId == productId);
            if (check != null)
            {
                db.Products.Remove(check);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            } else
                ViewBag.RemoveError = "Sản phẩm không tồn tại";
            return View();
        }
        public ActionResult EditProduct(Product model)
        {
            var check = db.Products.FirstOrDefault(s => s.ProductId == model.ProductId);
            if (check != null)
            {
                check.ProductId = model.ProductId;
                check.Name = model.Name;
                check.Price = model.Price;
                check.Color = model.Color;
                check.Brand = model.Brand;
                check.IsAvailable = model.IsAvailable;
                return View("Index");
            }
            return View();
        }
    }
}