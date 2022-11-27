using ShoesSalePage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;
using System.Web.UI;
using System.EnterpriseServices;
using System.Data;
using System.Drawing.Printing;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Data.Entity.SqlServer;
using WebGrease.Css.Ast;

namespace ShoesSalePage.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        private readonly ShoesSalePageEnityEntities db = new ShoesSalePageEnityEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Shop(int? page, string Cate, int? subCateId, string filter)
        {
            int pageSize, pageNumber;
            var product = db.Products.OrderBy(x => x.Price);
            if (Cate != null)                                                                                                   /*--Category--Start*/
            {
                ViewBag.Cate = Cate;
                product = db.Products.Where(p => p.Category.CategoryName.Equals(Cate)).OrderBy(p => p.ProductId);
                if(filter == "low_to_high")                                                                                         /*--Price Filter--Start*/
                {
                    ViewBag.Filter = "low_to_high";
                    product = db.Products.Where(p => p.Category.CategoryName.Equals(Cate)).OrderBy(p => p.Price);
                }else if(filter == "high_to_low")
                {
                    ViewBag.Filter = "high_to_low";
                    product = db.Products.Where(p => p.Category.CategoryName.Equals(Cate)).OrderByDescending(p => p.Price);
                }                                                                                                                   /*--Price Filter--End*/
                if (page == null)
                    page = 1;
                pageSize = 9;
                pageNumber = page ?? 1;
                return View(product.ToPagedList(pageNumber, pageSize));
            }                                                                                                                   /*--Category--End*/
            if (subCateId != null)                                                                                              /*--Sub-Category--Start*/
            {
                ViewBag.SubCate = subCateId;
                product = db.Products.Where(p => p.SubCategory.SubCategoryId == subCateId).OrderBy(p => p.ProductId);
                if (filter == "low_to_high")                                                                                        /*--Price Filter--Start*/
                {
                    ViewBag.Filter = "low_to_high";
                    product = db.Products.Where(p => p.SubCategory.SubCategoryId == subCateId).OrderBy(p => p.Price);
                }
                else if (filter == "high_to_low")
                {
                    ViewBag.Filter = "high_to_low";
                    product = db.Products.Where(p => p.SubCategory.SubCategoryId == subCateId).OrderByDescending(p => p.Price);
                }                                                                                                                   /*--Price Filter--End*/
                if (page == null)
                    page = 1;   
                pageSize = 9;
                pageNumber = page ?? 1;
                return View(product.ToPagedList(pageNumber, pageSize));
            }                                                                                                                   /*--Sub-Category--End*/
            if (filter == "low_to_high")                                                                                        /*--Shop--Start*/
            {                                                                                                                       /*--Price Filter--Start*/
                ViewBag.Filter = "low_to_high";
                product = db.Products.OrderBy(p => p.Price);
            }else if (filter == "high_to_low"){
                ViewBag.Filter = "high_to_low";
                product = db.Products.OrderByDescending(p => p.Price);
            }                                                                                                                       /*--Price Filter--End*/
            if (page == null)
                page = 1;
            pageSize = 9;
            pageNumber = page ?? 1;
            return View(product.ToPagedList(pageNumber, pageSize));                                                             /*--Shop--End*/
        }
        public ActionResult Search(int? page, string input, string filter)
        {
            ViewBag.Search = input;
            var item = from s in db.Products
                       select s;
            List<string> words = input.Split(' ').ToList();
            if (!String.IsNullOrEmpty(input))   
            {
                foreach(var word in words)
                {     
                    item = item.Where(s => s.Name.Contains(word)).OrderBy(p=>p.ProductId);
                    if(filter == "low_to_high")
                    {
                        ViewBag.Filter = "low_to_high";
                        item = item.Where(s => s.Name.Contains(word)).OrderBy(p => p.Price);
                    }else if(filter == "high_to_low")
                    {
                        ViewBag.Filter = "high_to_low";
                        item = item.Where(s => s.Name.Contains(word)).OrderByDescending(p => p.Price);
                    }
                }
            }
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;
            return View(item.ToPagedList(pageNumber, pageSize));
        }
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
        public ActionResult DetailsAddToCart(int id, string size, int? quantity)
        {
            var check = db.Stocks.Where(x => x.ProductId == id && x.Size1.Size1.Equals(size)).FirstOrDefault();
            if(quantity > check.Stock1 || quantity < 0)
                return RedirectToAction("Details", "Products", new { id = id });
            if (check == null)
                return RedirectToAction("Details", "Products", new {id = id});
            if (Session["Cart"] == null && check != null && quantity <= check.Stock1)
            {
                List<Cart> list = new List<Cart>();
                list.Add(new Cart { Product = db.Products.Find(id), Size = size, Quantity = quantity, CreatedDate = DateTime.Now }) ;
                Session["Cart"] = list;     // Store cart list to a session
                Session["Count"] = quantity;
            }
            else if (Session["Cart"] != null && check != null && quantity <= check.Stock1)
            {
                List<Cart> list = (List<Cart>)Session["Cart"];
                int index = CheckExist(id);
                if (index != -1)
                {
                    list[index].Quantity+= quantity;
                }
                else
                    list.Add(new Cart { Product = db.Products.Find(id), Size = size, Quantity = quantity, CreatedDate = DateTime.Now });
                Session["Cart"] = list;
                Session["Count"] = Convert.ToInt32(Session["Count"]) + quantity;
            }
            return RedirectToAction("Details", "Products", new {id = id});
        }
        public int CheckExist(int id)
        {
            List<Cart> list = (List<Cart>)Session["Cart"];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ProductID == id)
                    return i - 1;
            }
            return -1;
        }
        public ActionResult ColorFilter(int? page, string color, string filter)
        {
            ViewBag.Color = color;
            int pageSize, pageNumber;
            var product = db.Products.Where(p => p.Color.Equals(color)).OrderBy(p => p.ProductId);
            if(product != null)
            {
                if(filter == "low_to_high")
                {
                    ViewBag.Filter = filter;
                    product = db.Products.Where(p => p.Color.Equals(color)).OrderBy(p => p.Price);
                }else if(filter == "high_to_low")
                {
                    ViewBag.Filter = filter;
                    product = db.Products.Where(p => p.Color.Equals(color)).OrderByDescending(p => p.Price);
                }
            }
            if (page == null)
                page = 1;
            pageSize = 9;
            pageNumber = page ?? 1;
            return View(product.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SizeChart()
        {
            return PartialView("_SizeChart");
        }
        public ActionResult NewProduct()
        {
            var product = from s in db.Products
                          where SqlFunctions.DateDiff("day", DateTime.Now, s.CreatedDate) <= 7
                          orderby s.Price descending
                          select s;
            return PartialView("_NewProduct", product.Take(5).ToList());
        }
    }
}