using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebFPTShop.Models; 

namespace WebFPTShop.Controllers
{
    public class AdminCategoryController : Controller
    {
        private WebFPTShopEntities db = new WebFPTShopEntities();

        public ActionResult Index()
        {
            var categories = db.Categories.ToList();
            var categoryViewModels = new List<CategoryViewModel>();

            foreach (var category in categories)
            {
                int productCount = db.Products.Count(p => p.IDCate == category.IDCate);

                var categoryViewModel = new CategoryViewModel
                {
                    IDCate = category.IDCate,
                    NameCate = category.NameCate,
                    CateIcon = category.CateIcon,
                    ProductCount = productCount
                };

                categoryViewModels.Add(categoryViewModel);
            }

            return View(categoryViewModels);
        }


        public ActionResult Create()
        {
            return View(new Category()); 
        }


        // POST: AdminCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                // Thêm danh mục vào cơ sở dữ liệu và lưu thay đổi
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: AdminCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            // Tìm danh mục theo ID
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: AdminCategory/Edit/5
        [HttpPost]

        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                // Find the existing category in the database
                var existingCategory = db.Categories.Find(category.IDCate);

                if (existingCategory != null)
                {
                    // Update properties of the existing category
                    existingCategory.NameCate = category.NameCate;
                    existingCategory.CateIcon = category.CateIcon;
                    // Save changes to the database
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(category);
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return Json(new { success = false, message = "Danh mục không tồn tại." });
            }

            // Kiểm tra xem có bản ghi liên quan trong Products hay không
            if (db.Products.Any(p => p.IDCate == id))
            {
                return Json(new { success = false, message = "Không thể xóa danh mục có sản phẩm liên kết." });
            }

            // Kiểm tra xem có bản ghi liên quan trong các bảng khác (nếu có)

            // Nếu không có bản ghi liên quan, tiến hành xóa danh mục
            db.Categories.Remove(category);
            db.SaveChanges();

            return Json(new { success = true, message = "Xóa thành công." });
        }



        // Giải phóng DbContext khi không sử dụng nữa
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm danh mục theo ID với các sản phẩm liên quan
            Category category = db.Categories.Include(c => c.Products).SingleOrDefault(c => c.IDCate == id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

    }
}
