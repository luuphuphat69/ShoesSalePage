using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
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
                    try
                    {
                        List<Cart> listCart = (List<Cart>)Session["Cart"];
                        Order order = new Order();
                        order.UserId = (int)Session["UserID"];
                        db.Orders.Add(order);
                        db.SaveChanges();

                        foreach (Cart item in listCart)
                        {
                            Cart cart = new Cart();
                            cart.ProductID = item.Product.ProductId;
                            cart.Quantity = item.Quantity;
                            cart.Size = item.Size;
                            cart.CreatedDate = item.CreatedDate;
                            cart.OrderId = order.OrderId;

                            db.Carts.Add(cart);
                            db.SaveChanges();
                        }
                        Session["Cart"] = null;
                        Session["Count"] = null;
                        return RedirectToAction("ThankYou", "Carts");
                    }
                    catch (DbEntityValidationException e) //((System.Data.Entity.Validation.DbEntityValidationException)$exception).EntityValidationErrors.First().ValidationErrors.First().ErrorMessage
                    {   
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                }
            }
            return RedirectToAction("Login", "Users");
        }
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
