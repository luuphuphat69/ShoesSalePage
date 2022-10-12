using ShoesSalePage.Data;
using ShoesSalePage.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShoesSalePage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ShoesDbContext db = new ShoesDbContext();
            List<ShoesModel> shoes = db.Shoes.ToList();
            return View(shoes);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}   