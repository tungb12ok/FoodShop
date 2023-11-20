using FoodShop_SWP.Models;
using Microsoft.AspNetCore.Mvc;


namespace FoodShop_SWP.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class StatisticalController : Controller
    {
        
        private ShopFoodWebContext db = new ShopFoodWebContext();
        [Route("Statistical")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Statistical/GetStatistical")]
        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in db.Products
                        on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };
            var timeStart = DateTime.Now.AddDays(-7);
            fromDate = timeStart.ToString("dd/MM/yyyy");

            if (!string.IsNullOrEmpty(fromDate) && DateTime.TryParseExact(fromDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var startDate))
            {
                query = query.Where(x => x.CreatedDate >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate) && DateTime.TryParseExact(toDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var endDate))
            {
                query = query.Where(x => x.CreatedDate < endDate);
            }

            var result = query.GroupBy(x => x.CreatedDate.Date).Select(x => new
            {
                Date = x.Key,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });

            return Json(new { Data = result.ToList() });
        }

    }
}