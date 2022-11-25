using PagedList;
using ShoesSalePage.Models;
using System;
using System.Collections.Generic;
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
        public ActionResult ProductManagePage(int? page, string input)
        {
            if (Session["AdminID"] != null)                                                                         
            {
                if (page == null)
                    page = 1;
                int pageSize = 9;
                int pageNumber = page ?? 1;
                if (input != null)                                                                                  /*--Product Search--Start*/
                {
                    List<string> listWord = input.Split(' ').ToList();
                    ViewBag.SearchProduct = input;
                    foreach(var word in listWord)
                    {
                        var SearchProduct = from s in db.Products
                                            where s.ProductId.ToString().Contains(word) || s.Name.Contains(word) || s.Brand.Contains(word) ||
                                                  s.Price.ToString().Contains(word) || s.Color.Contains(word)
                                            orderby s.ProductId
                                            select s;
                        return View(SearchProduct.ToPagedList(pageNumber, pageSize));
                    }                                                                                               /*--Product Search--End*/
                }
                var product = db.Products.OrderBy(x => x.ProductId);
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
                var checkPhone = db.Admins.Where(s => s.PhoneNumber.Equals(admin.PhoneNumber));
                var checkAccount = db.Admins.Where(s => s.AdminName.Equals(admin.AdminName));
                if (checkPhone == null && checkAccount == null)
                {
                    admin.AdminPassword = GetMD5(admin.AdminPassword);
                    db.Configuration.ValidateOnSaveEnabled = false; // Disable entity validation
                    db.Admins.Add(admin);
                    db.SaveChanges();
                    return RedirectToAction("AdminProfile", "Admin");
                }else if(checkPhone != null)
                {
                    ViewBag.AdminPhoneMessage = "Số điện thoại đã được sử dụng";
                    return View();
                }else if(checkAccount != null)
                {
                    ViewBag.AdminAccountMessage = "Account is used. Try another name";
                    return View();
                }else
                {
                    ViewBag.AdminPhoneMessage = "Số điện thoại đã được sử dụng";
                    ViewBag.AdminAccountMessage = "Account is used. Try another name";
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
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Product product, HttpPostedFileBase file)
        {
            var check = db.Products.FirstOrDefault(s => s.ProductId == product.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", product.SubCategoryId);
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
                    return RedirectToAction("ProductManagePage", "Admin");
                }
            }
            else
                ViewBag.AddError = "ID đã tồn tại";

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
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
            return View(product);
        }

        // POST: _Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct([Bind(Include = "ProductId,Name,Price,Size,Brand,Color,Image,IsAvailable, CategoryId, SubCategoryId")] Product product, HttpPostedFileBase file)
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
                ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", product.SubCategoryId);

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductManagePage");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public ActionResult ShowCart(int? page, int? OrderId, string DateFilter)
        {
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;
            ViewBag.OrderId = OrderId;
            ViewBag.DateFilter = DateFilter;
            if(DateFilter != null)                                              /*--Date Filter--Start*/
            {
                if(DateFilter == "Lastest")
                {
                    var SearchCart_DateFilter = from s in db.Carts
                                     join k in db.Products on s.ProductID
                                     equals k.ProductId
                                     orderby s.CreatedDate descending
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
                    return View(SearchCart_DateFilter.ToPagedList(pageNumber, pageSize));
                }
                    var SearchCart = from s in db.Carts
                                         join k in db.Products on s.ProductID
                                         equals k.ProductId
                                         orderby s.CreatedDate ascending    
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
                    return View(SearchCart.ToPagedList(pageNumber, pageSize));
            }                                                                   /*--Date Filter--End*/
            if(OrderId != null)                                                 /*--OrderID Search--Start*/
            {
                var SearchCart = from s in db.Carts
                                 join k in db.Products on s.ProductID
                                 equals k.ProductId
                                 orderby s.CartID
                                 where s.OrderId == OrderId
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
                return View(SearchCart.ToPagedList(pageNumber, pageSize));
            }                                                                   /*--OrderID Search--End*/
            var cart = from s in db.Carts                                       /*--Show Cart--Default*/
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
            return View(cart.ToPagedList(pageNumber, pageSize));                /*--End--*/
        }
        public ActionResult ShowOrder(int? page, int? OrderId)
        {
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;
            ViewBag.SearchOrder = OrderId;
            if(OrderId != null)
            {
                var SearchOrder = from s in db.Orders
                            join k in db.Users on s.UserId
                            equals k.UserId
                            orderby s.OrderId
                            where s.OrderId == OrderId
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
                return View(SearchOrder.ToPagedList(pageNumber, pageSize));
            }
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
            return View(order.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ShowUser(int? page, string input)
        {
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;

            if(input != null)
            {
                ViewBag.SearchUser = input;
                List<string> list = input.Split(' ').ToList();
                if(!String.IsNullOrEmpty(input))
                {
                    foreach (var word in list)
                    {
                        var SearchUser = from s in db.Users
                                         where s.UserId.ToString().Contains(input) || s.UserName.Contains(input) || s.FirstName.Contains(input) ||
                                               s.LastName.Contains(input) || s.PhoneNumber.Contains(input) || s.Address.Contains(input)
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
                        return View(SearchUser.ToPagedList(pageNumber, pageSize));
                    }
                }
            }

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
        public ActionResult ProductStocks(int? page, string input)
        {
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;

            if(input != null)
            {
                ViewBag.StockSearch = input;
                List<string> words = input.Split(' ').ToList();
                foreach(var word in words)
                {
                    var stockProduct = from s in db.Stocks
                                       where s.Product.Name.Contains(word) || s.ProductId.ToString().Contains(word)
                                       orderby s.StockId
                                       select s;
                    return View(stockProduct.ToPagedList(pageNumber, pageSize));
                }
            }
            var product = db.Stocks.OrderBy(p => p.StockId);
            return View(product.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult AddProduct_ToStock(Stock StockProduct)
        {
            var check = db.Stocks.Where(p => p.ProductId == StockProduct.ProductId);
            if (check != null)
            {
                ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", StockProduct.ProductId);

                db.Stocks.Add(StockProduct);
                db.SaveChanges();
                return RedirectToAction("ProductStocks", "Admin");
            }
            else
                return View(StockProduct);
        }
        [HttpGet]
        public ActionResult AddProduct_ToStock()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductId", "Name");
            return View();
        }
        [HttpGet]
        public ActionResult EditStock(int? StockId)
        {
            var stock = db.Stocks.Where(p => p.StockId == StockId).FirstOrDefault();
            return View(stock);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStock(Stock stock)
        {
            var stocks = db.Stocks.Where(p => p.StockId == stock.StockId && p.ProductId == stock.ProductId).FirstOrDefault();
            if(stocks != null)
            {   
                stocks.ProductId = stock.ProductId;
                stocks.Stock1 = stock.Stock1;
                stock.Color = stock.Color;
                db.Entry(stocks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductStocks", "Admin");
            }
            return View();
        }
        [HttpGet]
        public ActionResult RemoveStock(int? StockId)
        {
            if (StockId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(StockId);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveStock(int StockId)
        {
            if(ModelState.IsValid)
            {
                Stock stock = db.Stocks.Find(StockId);
                db.Stocks.Remove(stock);
                db.SaveChanges();
                return RedirectToAction("ProductStocks", "Admin");
            }
            return View();
        }
    }
}