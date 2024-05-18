using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFPTShop.Models; 

namespace WebFPTShop.Controllers
{
    public class AdminProductController : Controller
    {
        private WebFPTShopEntities db = new WebFPTShopEntities(); 


        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).ToList();
            return View(products);
       
        }
        public ActionResult Details(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == id);
            var tenDangNhap = Session["TenDangNhap"] as string;
            ViewBag.TenDangNhap = tenDangNhap;
            if (product == null)
            {
                return HttpNotFound();
            }
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            return View(product);

        }



        private SelectList GetManuSelectList()
        {
            var categories = db.Manufacturers.ToList();
            return new SelectList(categories, "IDManu", "NameManu");
        }
        private SelectList GetCategorySelectList()
        {
            var categories = db.Categories.ToList(); 
            return new SelectList(categories, "Idcate", "NameCate");
        }
        public ActionResult Create()
        {
            ViewBag.CategorySelectList = GetCategorySelectList();
            ViewBag.ManuList = GetManuSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string directoryPath = Server.MapPath("~/Content/Images");

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string path = Path.Combine(directoryPath, fileName);
                    ImageFile.SaveAs(path);
                    model.ImagePro = fileName; 
                }
                model.SoLuongBan = 0;
                db.Products.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManuList = GetManuSelectList();
            ViewBag.CategorySelectList = GetCategorySelectList();
            return View(model);
        }

        // GET: AdminProduct/Edit/{id}
        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManuList = GetManuSelectList();
            ViewBag.CategorySelectList = GetCategorySelectList(); 
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product model, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string imagePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    string directoryPath = Server.MapPath("~/Content/Images");

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    ImageFile.SaveAs(imagePath);

                    model.ImagePro = fileName;
                }

                var existingProduct = db.Products.Find(model.ProductID);

                if (existingProduct != null)
                {
                    existingProduct.NamePro = model.NamePro;
                    existingProduct.DescriptionPro = model.DescriptionPro;
                    existingProduct.IDCate = model.IDCate;
                    existingProduct.Price = model.Price;
                    existingProduct.SoLuongBan = existingProduct.SoLuongBan;
                    existingProduct.SoLuongTon = model.SoLuongTon;
                    existingProduct.Screen = model.Screen;
                    existingProduct.Camera = model.Camera;
                    existingProduct.CameraSelfie = model.CameraSelfie;
                    existingProduct.CPU = model.CPU;
                    existingProduct.Store = model.Store;

                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        existingProduct.ImagePro = model.ImagePro;
                    }

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return HttpNotFound();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
                return View("Error"); 
            }
        }







        [HttpPost]
        public JsonResult Delete(int id)
        {
            var product = db.Products.Find(id);

            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại." });
            }

            try
            {
                if (product.OrderDetails.Any() || product.Ratings.Any())
                {
                    return Json(new { success = false, message = "Không thể xóa sản phẩm có liên kết." });
                }

                db.Products.Remove(product);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi khi xóa sản phẩm: {ex.Message}" });
            }
        }


    }
}
