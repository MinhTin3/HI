using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFPTShop.Models;

namespace WebFPTShop.Controllers
{
    public class AdminHomeController : Controller
    {
        private WebFPTShopEntities db = new WebFPTShopEntities();

        public ActionResult Index()
        {
            var products = db.OrderDetails
                .GroupBy(od => od.IDProduct)
                .Select(g => new TopSellingProductViewModel 
                {
                    ProductID = (int)g.Key,
                    NamePro = g.FirstOrDefault().Product.NamePro,
                    ImagePro = g.FirstOrDefault().Product.ImagePro,
                    QuantitySold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(10)
                .ToList();

            return View(products);
        }


    }
}
