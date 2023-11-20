using FoodShop_SWP.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop_SWP.Controllers
{
    public class BlogController : Controller
    {
        private readonly ShopFoodWebContext _context;

        public BlogController()
        {
            _context = new ShopFoodWebContext();
        }

        public IActionResult Index(string num)
        {
            int pageSize = 6;
            int page = 0;
            var listBlog = _context.News.Where(x => x.IsActive).ToList();
            page = listBlog.Count % 6 == 0 ? listBlog.Count / 6 : (listBlog.Count / 6) + 1;
            if (!String.IsNullOrEmpty(num))
            {
                listBlog = listBlog.Skip(pageSize * (Convert.ToInt32(num) - 1)).Take(pageSize).ToList();
            }
            else
            {
                num = "1";
                listBlog = listBlog.Skip(pageSize * (Convert.ToInt32(num) - 1)).Take(pageSize).ToList();
            }

            ViewData["list"] = listBlog;
            ViewData["page"] = page;
            return View();
        }

        public IActionResult BlogDetail(int id)
        {
            var blog = _context.News.FirstOrDefault(item => item.Id == id);
            if (blog != null)
            {
                ViewBag.Detail = blog.Detail;
                ViewData["blog"] = blog;
            }
            else
            {
                ViewData["blog"] = null;
            }

            return View();
        }
    }
}
