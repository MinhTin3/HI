using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFPTShop.Models;
using System.Data.Entity;
namespace WebFPTShop.Controllers
{
    public class HomeController : Controller
    {
        private WebFPTShopEntities db = new WebFPTShopEntities();
        public ActionResult Index()
        {
                var tenDangNhap = Session["TenDangNhap"] as string;
                ViewBag.TenDangNhap = tenDangNhap;
                List<Product> products = db.Products.ToList();
                var categories = db.Categories.ToList();
                ViewBag.Categories = categories;
                return View(products);
            
        }
   
        public ActionResult Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return RedirectToAction("Index");
            var tenDangNhap = Session["TenDangNhap"] as string;
            ViewBag.TenDangNhap = tenDangNhap;

            var products = db.Products.ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                products = products.Where(p =>
                    p.NamePro.ToLower().Contains(searchTerm) ||
                    p.NamePro.ToLower().Contains(searchTerm)
                ).ToList();
            }

            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            ViewBag.SearchTerm = searchTerm;
            return View(products);
        }

        public ActionResult Login()
        {
            var tenDangNhap = Session["TenDangNhap"] as string;
            ViewBag.TenDangNhap = tenDangNhap;
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer model)
        {

            if (ModelState.IsValid)
            {
                var categories = db.Categories.ToList();
                ViewBag.Categories = categories;
                var user = db.Customers.FirstOrDefault(u => u.EmailCus == model.EmailCus && u.PassCus == model.PassCus);

                if (user != null)
                {
                    Session["TenDangNhap"] = user.EmailCus;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            var tenDangNhap = Session["TenDangNhap"] as string;
            ViewBag.TenDangNhap = tenDangNhap;
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer model, string confirmPassword)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            if (ModelState.IsValid)
            {
                var existingUser = db.Customers.FirstOrDefault(u => u.EmailCus == model.EmailCus);

                if (existingUser == null)
                {
                    if (model.PassCus == confirmPassword)
                    {
                        var newUser = new Customer
                        {
                            NameCus = "New user",
                            EmailCus = model.EmailCus,
                            PassCus = model.PassCus
                        };

                        db.Customers.Add(newUser);
                        db.SaveChanges();

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Xác nhận mật khẩu không khớp.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập đã tồn tại. Vui lòng chọn tên đăng nhập khác.");
                }
            }

            return View(model);
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
        public ActionResult ProductsByCategory(string categoryName)
        {
            var tenDangNhap = Session["TenDangNhap"] as string;
            ViewBag.TenDangNhap = tenDangNhap;

            var products = db.Products
                .Include(p => p.Category)
                .Where(p => p.Category.NameCate == categoryName)
                .ToList();
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            return View(products);
        }
        private List<CartItem> GetCartItems()
        {
            return Session["Cart"] as List<CartItem> ?? new List<CartItem>();
        }

        private void SaveCartItems(List<CartItem> cartItems)
        {

            Session["Cart"] = cartItems;
        }

        public ActionResult ViewCart()
        {
            var cartItems = GetCartItems();


            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;

            return View(cartItems);
        }





        public ActionResult AddToCart(int productId)
        {
            using (var db = new WebFPTShopEntities())
            {
                var product = db.Products.FirstOrDefault(p => p.ProductID == productId);

                if (product == null)
                {
                    return HttpNotFound(); 
                }

                var cartItems = GetCartItems();

                var existingItem = cartItems.FirstOrDefault(item => item.Product.ProductID == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    var newItem = new CartItem { Product = product, Quantity = 1 };
                    cartItems.Add(newItem);
                }

                SaveCartItems(cartItems);

                return RedirectToAction("ViewCart");
            }
        }

        public ActionResult RemoveFromCart(int productId)
        {
            var cartItems = GetCartItems();

            var itemToRemove = cartItems.FirstOrDefault(item => item.Product.ProductID == productId);

            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                SaveCartItems(cartItems);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Checkout(List<CartItem> cartItems, string ship, string gender, string name, string phoneNumber, string email, string address)
        {
            var tenDangNhap = Session["TenDangNhap"] as string;
            if (tenDangNhap == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (cartItems == null || cartItems.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                Customer cs = db.Customers.FirstOrDefault(c => c.EmailCus == tenDangNhap);
                cs.NameCus = name;
                cs.PhoneCus = phoneNumber;
                cs.AddressCus = address;
                cs.VocativeCus = gender;
                db.SaveChanges();
                var customerId = cs.IDCus;
                var order = new OrderPro
                {
                    DateOrder = DateTime.Now,
                    AddressDeliverry = address,
                    IDCus = customerId
                };

                db.OrderProes.Add(order);
                db.SaveChanges();

                foreach (var item in cartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        IDProduct = item.Product.ProductID, 
                        IDOrder = order.ID,
                        Quantity = item.Quantity,
                        UnitPrice = (long)item.Product.Price 
                    };

                    db.OrderDetails.Add(orderDetail);
                }


                db.SaveChanges();

                Session["Cart"] = null;
                return RedirectToAction("OrderSuccess", new { orderId = order.ID });

            }
            catch (Exception ex)
            {


                ModelState.AddModelError(string.Empty, "An error occurred while processing your order. Please try again.");
                return RedirectToAction("Index", "Cart");
            }
        }



        public ActionResult OrderSuccess(int orderId)
        {
            var order = db.OrderProes.Find(orderId);
            var cartItems = Session["Cart"] as List<CartItem> ?? new List<CartItem>();


            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;

            if (order != null)
            {
                ViewBag.Order = order;
                return View(cartItems);
            }
            ViewBag.Order = null;
            TempData["ErrorMessage"] = "Order not found.";
            return View(cartItems);
        }
        [HttpPost]
        public ActionResult UpdateCartItemQuantity(int productId, int newQuantity)
        {
            var cartItems = GetCartItems();

            var cartItem = cartItems.FirstOrDefault(item => item.Product.ProductID == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = newQuantity;

                SaveCartItems(cartItems);
            }

            return Json(new { success = true });
        }

    }
}