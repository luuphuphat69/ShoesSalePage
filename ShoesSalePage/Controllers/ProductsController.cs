using ShoesSalePage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;

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
        public ActionResult Shop(int? page)
        {
            var product = db.Products.OrderBy(x => x.Price);
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = page ?? 1;
            return View(product.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Search(string input)
        {
            var item = from s in db.Products
                       select s;
            if (!String.IsNullOrEmpty(input))
            {
                item = item.Where(s => s.Name.Contains(input));
            }
            return View(item.ToList());
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
        public ActionResult AddToCart(int id)
        {
            if (Session["Cart"] == null)
            {
                List<Cart> list = new List<Cart>();
                list.Add(new Cart { Product = db.Products.Find(id), Quantity = 1, CreatedDate = DateTime.Now });
                Session["Cart"] = list;     // Store cart list to a session
                Session["Count"] = 1;
            }
            else
            {
                List<Cart> list = (List<Cart>)Session["Cart"];
                int index = CheckExist(id);
                if (index != -1)
                {
                    list[index].Quantity++;
                }
                else
                    list.Add(new Cart { Product = db.Products.Find(id), Quantity = 1, CreatedDate = DateTime.Now });
                Session["Cart"] = list;
                Session["Count"] = Convert.ToInt32(Session["Count"]) + 1;
            }
            return RedirectToAction("Shop", "Products");
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
    }
}