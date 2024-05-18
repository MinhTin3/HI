using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFPTShop.Models;

namespace WebFPTShop.Controllers
{
    public class AdminOrderProController : Controller
    {
        private WebFPTShopEntities db = new WebFPTShopEntities();

        // GET: OrderPro
        public ActionResult Index()
        {
            var orders = db.OrderProes.ToList();

            foreach (var order in orders)
            {
                var customer = db.Customers.FirstOrDefault(c => c.IDCus == order.IDCus);
                if (customer != null)
                {
                    order.Customer.NameCus = customer.NameCus;
                }
            }

            return View(orders);
        }


        // GET: OrderPro/Details/{id}
        public ActionResult Details(int id)
        {
            var order = db.OrderProes
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.ID == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            var customer = db.Customers.FirstOrDefault(c => c.IDCus == order.IDCus);
            if (customer != null)
            {
                order.Customer.NameCus = customer.NameCus;
            }
            foreach (var orderDetails in order.OrderDetails)
            {
                var product = db.Products.FirstOrDefault(p => p.ProductID == orderDetails.IDProduct);
                if (product != null)
                {
                    orderDetails.Product.ImagePro = product.ImagePro;
                    orderDetails.Product.NamePro = product.NamePro;
                }
            }
            return View(order);
        }


 
    }
}