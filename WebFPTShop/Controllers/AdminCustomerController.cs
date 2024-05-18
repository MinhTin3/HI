using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFPTShop.Models;
using System.Data.Entity;
using System.Net;

namespace WebFPTShop.Controllers
{
    public class AdminCustomerController : Controller
    {
        private WebFPTShopEntities db = new WebFPTShopEntities();

        // GET: Customer
        public ActionResult Index()
        {
            var customers = db.Customers.ToList();
            return View(customers);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            // Retrieve orders for the customer
            var customerOrders = db.OrderProes.Where(o => o.IDCus == id).ToList();

            // Pass customer and their orders to the view
            ViewBag.Customer = customer;

            return View(customerOrders);
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