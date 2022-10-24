using ShoesSalePage.Data;
using ShoesSalePage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace ShoesSalePage.Controllers
{
    /* Note: The ASP.NET Session is a convenient place to store user-specific information 
       which will expire after they leave the site. While misuse of session state can have performance implications
       on larger sites, our light use will work well for demonstration purposes.*/
    public class CartController : Controller
    {
        private readonly ShoesDbContext db = new ShoesDbContext();
        private readonly UsersDbContext userDb = new UsersDbContext();
        private readonly OrdersDbContext orderDb = new OrdersDbContext();
        public ActionResult Cart()
        {
            if (Session["Cart"] == null)
                return RedirectToAction("Shop", "Shoes");
            return View();
        }
        public ActionResult Add(int id)
        {
            if (Session["Cart"] == null)
            {
                List<Cart> list = new List<Cart>();
                list.Add(new Cart { Shoes = db.Shoes.Find(id), Quantity = 1, CreatedDate = DateTime.Now });
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
                    list.Add(new Cart { Shoes = db.Shoes.Find(id), Quantity = 1, CreatedDate = DateTime.Now });
                Session["Cart"] = list;
                Session["Count"] = Convert.ToInt32(Session["Count"]) + 1;
            }
            return RedirectToAction("Shop", "Shoes");
        }
        public ActionResult Remove(int id)
        {
            int count = 0;

            List<Cart> list = (List<Cart>)Session["Cart"];
            int index = CheckExist(id);
            list.RemoveAt(index + 1);
            Session["Cart"] = list;
            foreach(var item in list)
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
                if (list[i].Shoes.Id == id)
                    return i - 1;
            }
            return -1;
        }
        public ActionResult ConfirmOrder()
        {
            if(ModelState.IsValid)
            {

                if (Session["UserID"] != null)
                {
                    Order order = new Order();
                    List<Cart> carts = (List<Cart>)Session["Cart"];
                    order.Cart = carts;
                    orderDb.Orders.Add(order);
                    orderDb.SaveChanges();
                    Session["Cart"] = null;
                    Session["Count"] = null;
                    var id = from s in orderDb.Orders
                             select s.ID;
                    Session["OrderID"] = id.ToString();
                    return RedirectToAction("Shop", "Shoes");
                }
            }
            return RedirectToAction("Index", "Users");
        }
    }
}