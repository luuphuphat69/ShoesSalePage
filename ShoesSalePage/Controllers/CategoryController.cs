using PagedList;
using ShoesSalePage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ShoesSalePage.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        private readonly ShoesSalePageEnityEntities db = new ShoesSalePageEnityEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Men(int? page)
        {
            int pageSize = 9;
            //var menWear = db.Products.Where(s => s.Name.Contains("nam")).OrderBy(p => p.ProductId);
            var menWear = db.Products.Where(s => s.CategoryId == 1).OrderBy(p => p.ProductId);
            if (page == null)
                page = 1;
            int pageNumber = page ?? 1;
            return View(menWear.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Woman(int? page)
        {
            //var womanWear = db.Products.Where(s => s.Name.Contains("nữ")).OrderBy(p=>p.ProductId);
            var womanWear = db.Products.Where(s => s.CategoryId == 2).OrderBy(p => p.ProductId);
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;
            return View(womanWear.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Accessories(int? page)
        {
            /*var asc = db.Products.Where(s => s.Name.Contains("Giày") == false)
                                     .OrderBy(p => p.ProductId);*/
            var asc = db.Products.Where(s => s.CategoryId == 3).OrderBy(p => p.ProductId);
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;
            return View(asc.ToPagedList(pageNumber, pageSize));
        }
    }
}