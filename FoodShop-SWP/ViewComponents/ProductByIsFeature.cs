using FoodShop_SWP.Models;
using FoodShop_SWP.Models.EF;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop_SWP.ViewComponents
{
    public class ProductByIsFeature : ViewComponent
    {
        private readonly ShopFoodWebContext _context;
        public ProductByIsFeature(ShopFoodWebContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int cateId)
        {
            List<News> posts = _context.News.Take(3).ToList();
            ViewBag.posts = posts;
            var items = _context.Products.Where(x => x.IsFeature && x.IsActive).Where(x => x.Quantity>0).Take(12).ToList();
            return View(items);
        }
    }
}
