using System.Web.Mvc;

namespace ShoesSalePage.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Login()
        {
            return View();
        }
    }
}