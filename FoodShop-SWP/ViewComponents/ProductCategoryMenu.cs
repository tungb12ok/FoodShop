using FoodShop_SWP.Models;
using FoodShop_SWP.Models.EF;
using Microsoft.AspNetCore.Mvc;
namespace FoodShop_SWP.ViewComponents
{
    public class ProductCategoryMenu : ViewComponent
    {
        private readonly ShopFoodWebContext _context;
        public ProductCategoryMenu(ShopFoodWebContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            List<News> posts = _context.News.Take(3).ToList();
            ViewBag.posts = posts;
            var items = _context.ProductCategories.OrderBy(x => x.CreatedBy);
            return View(items);
        }
    }
}
