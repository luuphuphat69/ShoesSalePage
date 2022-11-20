using PagedList;
using ShoesSalePage.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ShoesSalePage.Controllers
{
    public class AdminController : Controller
    {
        private readonly ShoesSalePageEnityEntities db = new ShoesSalePageEnityEntities();
        public ActionResult ProductManagePage(int? page)
        {
            if (Session["AdminID"] != null)
            {
                var product = db.Products.OrderBy(x => x.ProductId);
                if (page == null)
                    page = 1;
                int pageSize = 9;
                int pageNumber = page ?? 1;
                return View(product.ToPagedList(pageNumber, pageSize));
            }
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
                if (check == null)
                {
                    admin.AdminPassword = GetMD5(admin.AdminPassword);
                    db.Configuration.ValidateOnSaveEnabled = false; // Disable entity validation
                    db.Admins.Add(admin);
                    db.SaveChanges();
                    return RedirectToAction("AdminProfile", "Admin");
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
            if (Session["AdminId"] != null)
                return RedirectToAction("AdminProfile", "Admin");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string AdminName, string password)
        {
            if (ModelState.IsValid)
            {
                var _password = GetMD5(password);
                var data = db.Admins.FirstOrDefault(s => s.AdminName.Equals(AdminName) && s.AdminPassword.Equals(_password));
                if (data == null)
                {
                    return View();
                }
                else
                Session["AdminID"] = data.AdminID;
                Session["FullName"] = data.FullName;
                Session["AdminName"] = data.AdminName;
                Session["PhoneNumber"] = data.PhoneNumber;
                return RedirectToAction("AdminProfile");
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("AdminProfile", "Admin");
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
       // [Authorize(Roles = "Admin")]
        public ActionResult AddProduct(Product product, HttpPostedFileBase file)
        {
            var check = db.Products.FirstOrDefault(s => s.ProductId == product.ProductId);
            if (check == null)
            {
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + "_" + Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images/ProductImg/"), fileName);

                        file.SaveAs(path);
                        product.Image = file.FileName;
                    }
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("ProductManangePage", "Admin");
                }
            }
            else
                ViewBag.AddError = "Sản phẩm đã tồn tại";

            return View();
        }
        // GET: _Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        // GET: _Products/EditProduct/5
        [HttpGet]
        public ActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: _Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       // [Authorize(Roles = "Admin")]
        public ActionResult EditProduct([Bind(Include = "ProductId,Name,Price,Size,Brand,Color,Image,IsAvailable")] Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid().ToString().Replace("-", "") + "_" + Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/ProductImg/"), fileName);

                    file.SaveAs(path);
                    product.Image = file.FileName;
                }
                //db.Products.Add(product);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("{roductManagePage");
            }
            return View(product);
        }
        // GET: _Products/Delete/5]
        public ActionResult RemoveProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: _Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult RemoveProduct(int id)
        {
            if (ModelState.IsValid)
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("ProductManagePage");
            }
            return RedirectToAction("ProductMangePage", "Admin");
        }
        public ActionResult ShowCart(int? page)
        {
            /*var order = db.Carts.OrderBy(p => p.OrderId);
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;
            return View(order.ToPagedList(pageNumber, pageSize));*/
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;

            var cart = from s in db.Carts
                       join k in db.Products on s.ProductID
                       equals k.ProductId
                       orderby s.CartID
                       select new CartViewModel
                       {
                           OrderId = s.OrderId,
                           ProductId = k.ProductId,
                           ProductName = k.Name,
                           Brand = k.Brand,
                           Price = k.Price,
                           Size = s.Size,
                           Quantity = s.Quantity,
                           CreatedDate = s.CreatedDate,
                       };
            return View(cart.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ShowOrder(int? page)
        {
            var order = from s in db.Orders
                       join k in db.Users on s.UserId
                       equals k.UserId
                       orderby s.OrderId
                       select new OrderViewModel
                       {
                           OrderId = s.OrderId,
                           UserId = k.UserId,

                           UserName = k.UserName,
                           FirstName = k.FirstName,
                           LastName = k.LastName,
                           PhoneNumber = k.PhoneNumber,
                           Email = k.Email,
                       };
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;
            return View(order.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ShowUser(int? page)
        {
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;

            var user = from s in db.Users
                       orderby s.UserId
                       select new UserViewModel
                       {
                           UserId = s.UserId,
                           UserName = s.UserName,
                           UserFullName = s.FirstName + " " + s.LastName,
                           UserAddress = s.Address + ", " + s.City,
                           UserEmail = s.Email,
                           UserPhoneNumber = s.PhoneNumber,
                           CreatedDate = s.CreatedDate,
                       };
            return View(user.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult AdminProfile()
        {
            if (Session["AdminID"] == null)
                return RedirectToAction("Login", "Admin");
            int adminId = (int)Session["AdminID"];
            Admin admin = db.Admins.FirstOrDefault(p => p.AdminID== adminId);
            return View(admin);
        }
    }
}