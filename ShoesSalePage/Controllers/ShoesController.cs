using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using ShoesSalePage.Data;
using System.Linq;
using ShoesSalePage.Models;
using PagedList;
using System;
using System.ComponentModel.DataAnnotations;

namespace ShoesSalePage.Controllers
{
    public class ShoesController : Controller
    {
        private readonly ShoesDbContext db = new ShoesDbContext();

        // GET: ShoesModels
        public ActionResult Shop(int? page)
        {
            var shoes = db.Shoes.OrderBy(p => p.Id);
            if (page == null)
                page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(shoes.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Search(string input)
        {
            var item = from s in db.Shoes
                       select s;
            if (!String.IsNullOrEmpty(input))
            {
                item = item.Where(s => s.Name.Contains(input));
            }
            return View(item.ToList());
        }
        // GET: ShoesModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoesModel shoesModel = db.Shoes.Find(id);
            if (shoesModel == null)
            {
                return HttpNotFound();
            }
            return View(shoesModel);
        }
        /*public ActionResult AddToCart()
        {
            return RedirectToAction("Buy", "Cart"); // (action, controller)
        }*/
        [Route("/removecart/{id:int}")]
        public ActionResult RemoveCart(int id)
        {
            return RedirectToAction("Buy", "Cart");
        }
        [HttpPost]
        [Route("/updatecart")]
        public ActionResult UpdateCart(int id, int quantity)
        {
            return RedirectToAction("Add", "Cart");
        }
        // GET: ShoesModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShoesModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Size,Brand,Color,Image,IsAvailable")] ShoesModel shoesModel)
        {
            if (ModelState.IsValid)
            {
                db.Shoes.Add(shoesModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shoesModel);
        }

        // GET: ShoesModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoesModel shoesModel = db.Shoes.Find(id);
            if (shoesModel == null)
            {
                return HttpNotFound();
            }
            return View(shoesModel);
        }

        // POST: ShoesModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Size,Brand,Color,Image,IsAvailable")] ShoesModel shoesModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoesModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shoesModel);
        }

        // GET: ShoesModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoesModel shoesModel = db.Shoes.Find(id);
            if (shoesModel == null)
            {
                return HttpNotFound();
            }
            return View(shoesModel);
        }

        // POST: ShoesModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoesModel shoesModel = db.Shoes.Find(id);
            db.Shoes.Remove(shoesModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
