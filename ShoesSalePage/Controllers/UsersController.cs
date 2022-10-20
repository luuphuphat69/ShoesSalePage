using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using ShoesSalePage.Data;
using ShoesSalePage.Models;

namespace ShoesSalePage.Controllers
{
    public class UsersController : Controller
    {
        private UsersDbContext db = new UsersDbContext();

        // GET: Users
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User users)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.Email == users.Email || s.PhoneNumber == users.PhoneNumber || s.UserName == users.UserName);
                if (check == null)
                {
                    users.Password = GetMD5(users.Password);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(users);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.PhoneError = "Số điện thoại đã được sử dụng hoặc chưa đăng ký";
                    ViewBag.MailError = "Mail đã được sử dụng hoặc mail chưa đăng ký";
                    ViewBag.UserNameError = "Tên tài khoản đã được sử dụng";
                    return View();
                }
            }
            return View();
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = db.Users.Where(s => s.UserName.Equals(userName) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["UserName"] = data.FirstOrDefault().UserName;
                    Session["PhoneNumber"] = data.FirstOrDefault().PhoneNumber;
                    Session["Address"] = data.FirstOrDefault().Address;
                    Session["City"] = data.FirstOrDefault().City;
                    Session["UserID"] = data.FirstOrDefault().UserId;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index", "Users");
        }
    }
}
