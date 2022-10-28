using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoesSalePage.Models;

namespace ShoesSalePage.Controllers
{
    public class CartsController : Controller
    {
        private readonly ShoesSalePageEnityEntities db = new ShoesSalePageEnityEntities();

        // GET: Carts
        public ActionResult Cart()
        {
            //var carts = db.Carts.Include(c => c.Order).Include(c => c.Product);
            //return View(carts.ToList());
            if (Session["Cart"] == null)
                return RedirectToAction("Shop", "Products");
            return View();
        }
        public ActionResult RemoveCart(int id)
        {
            int count = 0;

            List<Cart> list = (List<Cart>)Session["Cart"];
            int index = CheckExist(id);
            list.RemoveAt(index + 1);
            Session["Cart"] = list;
            foreach (var item in list)
            {
                count += item.Quantity;
            }
            Session["Count"] = count;
            return RedirectToAction("Cart");
        }
        private int CheckExist(int id)
        {
            List<Cart> list = (List<Cart>)Session["Cart"];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Product.ProductId == id)
                    return i - 1;
            }
            return -1;
        }
        public ActionResult ConfirmOrder()
        {
            if (ModelState.IsValid)
            {

                if (Session["UserID"] != null)
                {
                    Order order = new Order();
                    List<Cart> carts = (List<Cart>)Session["Cart"];
                    order.Carts = carts;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    Session["Cart"] = null;
                    Session["Count"] = null;
                    var id = from s in db.Orders
                             select s.OrderID;
                    Session["OrderID"] = id.ToString();
                    return RedirectToAction("Shop", "Products");
                }
            }
            return RedirectToAction("Index", "Users");
        }
    }
}
