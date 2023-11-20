using FoodShop_SWP.Models;
using FoodShop_SWP.Models.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShop_SWP.Areas.Admin.ViewComponents


{
    public class Partial_Product : ViewComponent
    {
        private readonly ShopFoodWebContext _context;
        public Partial_Product(ShopFoodWebContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int id)
        {
            List<OrderDetail> items = _context.OrderDetails.Include(x => x.Product).Where(x => x.OrderId == id).ToList();
            ViewBag.ProductTitle = items;
            return View();
        }
    }
}
