using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFPTShop.Models;
using System.Data.Entity;
namespace WebFPTShop.Controllers
{
    public class AdminManufacturerController : Controller
    {

        private WebFPTShopEntities db = new WebFPTShopEntities();

        public ActionResult Index()
        {
            var categories = db.Manufacturers.ToList();
            return View(categories);
        }

        public ActionResult Create()
        {
            return View(new Manufacturer());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                // Thêm danh mục vào cơ sở dữ liệu và lưu thay đổi
                db.Manufacturers.Add(manufacturer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manufacturer);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            // Tìm danh mục theo ID
            Manufacturer manufacturer = db.Manufacturers.Find(id);
            if (manufacturer == null)
            {
                return HttpNotFound();
            }
            return View(manufacturer);
        }

        [HttpPost]

        public ActionResult Edit(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                var existingManu = db.Manufacturers.Find(manufacturer.IDManu);

                if (existingManu != null)
                {
                    existingManu.NameManu = manufacturer.NameManu;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(manufacturer);
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            Manufacturer manufacturer = db.Manufacturers.Find(id);
            if (manufacturer == null)
            {
                return Json(new { success = false });
            }

            db.Manufacturers.Remove(manufacturer);
            db.SaveChanges();
            return Json(new { success = true });
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