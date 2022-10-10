using ShoesSalePage.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ShoesSalePage.Controllers
{
    public class CartController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}