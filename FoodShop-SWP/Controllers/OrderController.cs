using FoodShop_SWP.Models.EF;
using FoodShop_SWP.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace FoodShop_SWP.Controllers
{
    public class OrderController : Controller
    {
        ShopFoodWebContext db = new ShopFoodWebContext();
        [Route("Order/order")]
        public IActionResult Index(string Searchtext, int? page)
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("Login", "User");
            }
            string UserId = HttpContext.Session.GetString("UserId");
            int pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            //IEnumerable<News> items = db.News.OrderByDescending(x => x.Id);

            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var items = db.Orders.AsNoTracking().OrderByDescending(x => x.ModifiedDate);
            //if (!string.IsNullOrEmpty(Searchtext))
            //{
            //    items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            //}
            List<Order> list = db.Orders.Where(x => x.CreatedBy == UserId).ToList();
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            ViewBag.list = list;

            return View(list);
        }
        [Route("Order/View")]

        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }
        [Route("Order/Partial_SanPham")]

        public ActionResult Partial_SanPham(int id)
        {
            List<OrderDetail> items = db.OrderDetails.Include(x => x.Product).Where(x => x.OrderId == id).ToList();
            return PartialView(items);
        }
        [Route("Order/UpdateStatus")]
        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.Status = status;
                db.Entry(item).Property(x => x.Status).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
        }
    }
}
