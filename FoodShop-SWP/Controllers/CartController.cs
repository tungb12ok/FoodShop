using FoodShop_SWP.Common;
using FoodShop_SWP.Models;
using FoodShop_SWP.Models.Common;
using FoodShop_SWP.Models.EF;
using FoodShop_SWP.Ultis;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop_SWP.Controllers
{
    public class CartController : Controller
    {
        private readonly IVnPayService _vnPayService;

        public CartController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
        public CartCRUD? cartCRUD;
        ShopFoodWebContext _context = new ShopFoodWebContext();
        private readonly ISession _session;
        private Cart currentCart = new Cart();
        [HttpGet]
        public IActionResult Index(string handler, int productId, int quantity, string url)
        {
            cartCRUD = new CartCRUD(_context, HttpContext.Session);
            bool isLogged = false;
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("Login", "User");
            }
            switch (handler)
            {
                case "increment":
                    cartCRUD.UpdateQuantity(productId, isAdd: true);
                    break;
                case "decrement":
                    cartCRUD.UpdateQuantity(productId, isRemove: true);
                    break;
                case "delete":
                    cartCRUD.RemoveFromCart(productId);
                    break;
                case "clear":
                    cartCRUD.ClearCart();
                    break;
                case "add":
                    cartCRUD.AddToCart(productId, quantity);
                    break;
                default:
                    break;
            }
            var res = new { success = true, totalQty = cartCRUD.GetTotalQty() };
            ViewData["Cart"] = cartCRUD.GetCart();
            ViewData["IsLogged"] = isLogged;
            Cart cart = cartCRUD.GetCart();
            currentCart = cart;
            HttpContext.Session.SetString("countCart", cart.Count.ToString());
            switch (url)
            {
                case "ProductList":
                    return RedirectToAction("ProductList", "Product");
                case "ProductDetail":
                    return Redirect("/Product/ProductDetail?id=" + productId);
                case "Home":
                    return RedirectToAction("Index", "Home");
                case "ViewCart":
                    ViewBag.cart = cart;
                    return RedirectToAction("Index", "Cart");
                case "Checkout":
                    ViewBag.cart = cart;
                    return RedirectToAction("Checkout", "Cart");
                default:
                    return View();
            }
        }
        public IActionResult Checkout()
        {
            cartCRUD = new CartCRUD(_context, HttpContext.Session);
            ViewBag.cart = cartCRUD.GetCart();
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(string customerName, string email, string phone, string address, int paymentType)
        {
            cartCRUD = new CartCRUD(_context, HttpContext.Session);
            Cart cart = cartCRUD.GetCart();
            Order order = new Order();
            string oCode = Guid.NewGuid().ToString();
            order.Code = oCode;
            order.Email = email;
            order.CreatedDate = DateTime.Now;
            order.ModifiedDate = DateTime.Now;
            order.CustomerName = customerName;
            order.Phone = phone;
            order.CreatedBy = HttpContext.Session.GetString("UserId");
            order.Address = address;
            order.TypePayment = paymentType;
            if (paymentType != 1)
            {
                order.Status = 2;
            }
            else {order.Status = 1; }
            
            order.Quantity = cart.Count;
            order.TotalAmount = cart.Total;
            if (paymentType == 1)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                Order insertedOrder = _context.Orders.FirstOrDefault(x => x.Code == oCode);
                foreach (CartItem item in cart.CartItems)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.OrderId = insertedOrder.Id;
                    detail.ProductId = item.Product.Id;
                    if (item.Product.PriceSale > 0)
                    {
                        detail.Price = item.Product.PriceSale;
                    }
                    else
                    {
                        detail.Price = item.Product.Price;
                    }
                    detail.Quantity = item.Quantity;
                    _context.OrderDetails.Add(detail);
                    _context.SaveChanges();
                }
                ViewBag.cart = cartCRUD.GetCart();
                ViewBag.mess = "Checkout successfully. The product will be shipped to you soon!";
                cartCRUD.ClearCart();
                return RedirectToAction("ProductList", "Product");
            }
            else
            {
                PaymentInformationModel model = new PaymentInformationModel()
                {
                    Name = "Thanh toán cho đơn hàng",
                    Amount = Double.Parse(cart.Total.ToString()) * 100,
                    OrderDescription = "abc",
                    OrderType = "abc",

                };
                var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

                return Redirect(url);
            }

        }

        [Route("/PaymentCallback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }
    }
}