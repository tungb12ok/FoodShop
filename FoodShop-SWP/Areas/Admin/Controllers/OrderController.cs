using FoodShop_SWP.Models;
using FoodShop_SWP.Models.EF;
using FoodShop_SWP.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;
using System.Globalization;

namespace FoodShop_SWP.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class OrderController : Controller
    {
        ShopFoodWebContext db = new ShopFoodWebContext();
        [Route("order")]
        public IActionResult Index(string Searchtext, int? page)
        {

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
            PagedList<Order> list = new(items, pageNumber, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
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

//        public void ThongKe(string fromDate, string toDate)
//        {
//            var query = from o in db.Orders
//                        join od in db.OrderDetails on o.Id equals od.OrderId
//                        join p in db.Products
//on od.ProductId equals p.Id
//                        select new
//                        {
//                            CreatedDate = o.CreatedDate,
//                            Quantity = od.Quantity,
//                            Price = od.Price,
//                            OriginalPrice = p.Price
//                        };
//            if (!string.IsNullOrEmpty(fromDate))
//            {
//                DateTime start = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
//                query = query.Where(x => x.CreatedDate >= start);
//            }
//            if (!string.IsNullOrEmpty(toDate))
//            {
//                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
//                query = query.Where(x => x.CreatedDate < endDate);
//            }
//            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(r => new
//            {
//                Date = r.Key.Value,
//                TotalBuy = r.Sum(x => x.OriginalPrice * x.Quantity), // tổng giá bán
//                TotalSell = r.Sum(x => x.Price * x.Quantity) // tổng giá mua
//            }).Select(x => new RevenueStatisticViewModel
//            {
//                Date = x.Date,
//                Benefit = x.TotalSell - x.TotalBuy,
//                Revenues = x.TotalSell
//            });
//        }
    }
}
