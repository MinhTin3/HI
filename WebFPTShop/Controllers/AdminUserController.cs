using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFPTShop.Models;

namespace WebFPTShop.Controllers
{
    public class AdminUserController : Controller
    {
          private WebFPTShopEntities db = new WebFPTShopEntities();

        // GET: AdminUser
        public ActionResult Index()
        {
            var users = db.AdminUsers.ToList();
            return View(users);
        }

        // GET: AdminUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
                db.AdminUsers.Add(adminUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adminUser);
        }

        // GET: AdminUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            AdminUser adminUser = db.AdminUsers.Find(id);

            if (adminUser == null)
            {
                return HttpNotFound();
            }

            return View(adminUser);
        }

        // POST: AdminUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adminUser);
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            AdminUser adminUser = db.AdminUsers.Find(id);
            if (adminUser == null)
            {
                return Json(new { success = false });
            }

            db.AdminUsers.Remove(adminUser);
            db.SaveChanges();
            return Json(new { success = true });
        }

        public ActionResult Login()
        {
            return View();
        }

        // POST: AdminUser/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminUser model)
        {
            var user = db.AdminUsers.FirstOrDefault(u => u.NameUser == model.NameUser && u.PasswordUser == model.PasswordUser);

            if (user != null)
            {
                Session["AdminUser"] = user.NameUser;
                return RedirectToAction("Index", "AdminHome");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }
        /* protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        } */

        // GET: AdminUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            AdminUser adminUser = db.AdminUsers.Find(id);

            if (adminUser == null)
            {
                return HttpNotFound();
            }

            return View(adminUser);
        }
    }
}