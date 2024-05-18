using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFPTShop.Models;

namespace WebFPTShop.Controllers
{
    public class AdminWareHouseController : Controller
    {
        private WebFPTShopEntities db = new WebFPTShopEntities();

        public ActionResult Index()
        {

            var inStockProducts = db.Products.Where(p => p.SoLuongTon > 0).ToList();

            var productsToRestock = db.Products.Where(p => p.SoLuongTon == 0).ToList();

            ViewBag.InStockProducts = inStockProducts;
            ViewBag.ProductsToRestock = productsToRestock;

            return View();
        }
    }
}
