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
    public class FilterController : Controller
    {
        private ShoesSalePageEnityEntities db = new ShoesSalePageEnityEntities();

        // GET: Filter
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }
        /*public ActionResult FirstRange()
        {
            var product = db.Products.Where(p => p.Price >= 100000 || p.Price <= 500000);
            if()
        }*/
    }
}
