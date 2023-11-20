using FoodShop_SWP.Models;
using FoodShop_SWP.Models.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShop_SWP.Controllers
{
    public class ProductController : Controller
    {
        ShopFoodWebContext db = new ShopFoodWebContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductList(int? cid, string? search, int? index)
        {
            if (search == null)
            {
                search = "";
            }
            if (index == null)
            {
                index = 1;
            }
            index = index - 1;
            int pagesize = 9;
            int totalPage = 0;
            List<Product> saleProduct = db.Products.Include(x => x.ProductCategory).Where(x => x.IsSale == true && x.Quantity > 0).ToList();
            List<Product> lastProduct = db.Products.Include(x => x.ProductCategory).Where(x => x.IsSale == true && x.Quantity > 0).OrderByDescending(x => x.CreatedDate).Skip(0).Take(5).ToList();
            List<ProductCategory> cates = db.ProductCategories.ToList();
            List<Product> pl = new List<Product>();
            if (cid == null)
            {
                int totalRecord = db.Products.Include(x => x.ProductCategory).Where(x => x.Title.Contains(search) && x.Quantity > 0).Count();
                pl = db.Products.Where(x => x.Title.Contains(search)).Skip(index.Value * pagesize).Take(pagesize).ToList();
                totalPage = (int)Math.Ceiling((double)totalRecord / pagesize);

            }
            else
            {
                int totalRecord = db.Products.Include(x => x.ProductCategory).Where(x => x.Title.Contains(search) && x.ProductCategoryId == cid && x.Quantity > 0).Count();
                totalPage = (int)Math.Ceiling((double)totalRecord / pagesize);
                pl = db.Products.Where(x => x.Title.Contains(search) && x.ProductCategoryId == cid).Skip(index.Value * pagesize).Take(pagesize).ToList();

            }
            ViewBag.salePro = saleProduct;
            ViewBag.lastProduct = lastProduct;

            ViewBag.cates = cates;
            ViewBag.pl = pl;
            ViewBag.search = search;
            ViewBag.cid = cid;
            ViewBag.index = index;
            ViewBag.totalPage = totalPage;
            return View();
        }
        public IActionResult ProductDetail(int id)
        {
            Product product = db.Products.Include(x => x.ProductCategory).SingleOrDefault(x => x.Id == id);
            List<Product> relatedProduct = db.Products.Include(x => x.ProductCategory).Where(x => x.ProductCategoryId == product.ProductCategoryId).Skip(0).Take(4).ToList();
            ViewBag.ProductDetail = product.Detail;
            List<Feedback> feedbacks = db.Feedbacks.Include(n => n.user).Where(n => n.ProductID == id).ToList();
            ViewBag.product = product;
            ViewBag.relatedProduct = relatedProduct;
            ViewBag.feedbacks = feedbacks;
            return View();
        }
    }
}
