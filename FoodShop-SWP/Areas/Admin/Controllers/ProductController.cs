using FoodShop_SWP.Models.EF;
using FoodShop_SWP.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodShop_SWP.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class ProductController : Controller
    {
        ShopFoodWebContext db = new ShopFoodWebContext();
        [Route("product")]
        public IActionResult Index(string Searchtext, int? page)
        {



            int pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            //IEnumerable<News> items = db.News.OrderByDescending(x => x.Id);
            //if (!string.IsNullOrEmpty(Searchtext))
            //{
            //    items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            //}
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var items = db.Products.AsNoTracking().OrderByDescending(x => x.ModifiedDate);
            PagedList<Product> list = new(items, pageNumber, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(list);
        }
        [Route("product/Add")]
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View();
        }
        [Route("product/Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = FoodShop_SWP.Models.Common.Filter.FilterChar(model.Title);
                db.Products.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var validationErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.ValidationErrors = validationErrors;
            return View(model);
        }
        [Route("product/Edit")]
        [HttpGet]
        public IActionResult Edit(int id)
        {

            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            var item = db.Products.Find(id);
            return View(item);
        }
        [Route("product/Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                db.Products.Attach(model);
                model.ModifiedDate = DateTime.Now;
                model.Alias = FoodShop_SWP.Models.Common.Filter.FilterChar(model.Title);
                db.Entry(model).Property(x => x.Title).IsModified = true;
                db.Entry(model).Property(x => x.Image).IsModified = true;
                db.Entry(model).Property(x => x.ProductCode).IsModified = true;
                db.Entry(model).Property(x => x.ProductCategoryId).IsModified = true;
                db.Entry(model).Property(x => x.Alias).IsModified = true;
                db.Entry(model).Property(x => x.Description).IsModified = true;
                db.Entry(model).Property(x => x.Detail).IsModified = true;
                db.Entry(model).Property(x => x.Quantity).IsModified = true;
                db.Entry(model).Property(x => x.Price).IsModified = true;
                db.Entry(model).Property(x => x.OriginalPrice).IsModified = true;
                db.Entry(model).Property(x => x.PriceSale).IsModified = true;
                db.Entry(model).Property(x => x.IsSale).IsModified = true;
                db.Entry(model).Property(x => x.IsFeature).IsModified = true;
                db.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                db.Entry(model).Property(x => x.Modifiedby).IsModified = true;
                db.Entry(model).Property(x => x.IsActive).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var validationErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.ValidationErrors = validationErrors;
            return View(model);
        }
        [Route("product/Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        [Route("product/IsActive")]
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.ModifiedDate = DateTime.Now;
                item.IsActive = !item.IsActive;
                if(item.Quantity == 0)
                {
                    item.IsActive = false;
                }
                db.Entry(item).Property(x => x.ModifiedDate).IsModified = true;
                db.Entry(item).Property(x => x.IsActive).IsModified = true;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }

            return Json(new { success = false });
        }
        [Route("product/IsFeature")]
        [HttpPost]
        public ActionResult IsFeature(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.ModifiedDate = DateTime.Now;
                item.IsFeature = !item.IsFeature;
                db.Entry(item).Property(x => x.ModifiedDate).IsModified = true;
                db.Entry(item).Property(x => x.IsFeature).IsModified = true;
                db.SaveChanges();
                return Json(new { success = true, isFeature = item.IsFeature });
            }

            return Json(new { success = false });
        }
        [Route("product/IsSale")]
        [HttpPost]
        public ActionResult IsSale(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.ModifiedDate = DateTime.Now;
                item.IsSale = !item.IsSale;
                db.Entry(item).Property(x => x.ModifiedDate).IsModified = true;
                db.Entry(item).Property(x => x.IsSale).IsModified = true;
                db.SaveChanges();
                return Json(new { success = true, isSale = item.IsSale });
            }

            return Json(new { success = false });
        }
        [Route("product/DeleteAll")]
        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = db.Products.Find(Convert.ToInt32(item));
                        db.Products.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}