using ShoesSalePage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoesSalePage.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private readonly ShoesSalePageEnityEntities db = new ShoesSalePageEnityEntities();
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }
    }
}