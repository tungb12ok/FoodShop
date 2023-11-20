using FoodShop_SWP.Models;
using FoodShop_SWP.Models.EF;
using FoodShop_SWP.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace FoodShop_SWP.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class NewController : Controller
    {
        ShopFoodWebContext db = new ShopFoodWebContext();
        [Route("blog")]
        public IActionResult Index(string Searchtext, int? page)
        {
            IEnumerable<News> items = db.News.OrderByDescending(x => x.ModifiedDate);
            int pageSize = 6;
            if (page == null)
            {
                page = 1;
            }

            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            //var items = db.News.AsNoTracking().OrderByDescending(x => x.ModifiedDate);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }
            PagedList<News> list = new(items, pageNumber, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(list);
        }
        [Route("blog/Add")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [Route("blog/Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(News model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = FoodShop_SWP.Models.Common.Filter.FilterChar(model.Title);
                db.News.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var validationErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.ValidationErrors = validationErrors;
            return View(model);
        }
        [Route("blog/Edit")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = db.News.Find(id);
            return View(item);
        }
        [Route("blog/Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
                //int pageNumber = 2;
                db.News.Attach(model);
                model.ModifiedDate = DateTime.Now;
                model.Alias = FoodShop_SWP.Models.Common.Filter.FilterChar(model.Title);
                db.Entry(model).Property(x => x.Title).IsModified = true;
                db.Entry(model).Property(x => x.Image).IsModified = true;
                db.Entry(model).Property(x => x.Detail).IsModified = true;
                db.Entry(model).Property(x => x.Description).IsModified = true;
                db.Entry(model).Property(x => x.Alias).IsModified = true;
                db.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                db.Entry(model).Property(x => x.Modifiedby).IsModified = true;
                db.Entry(model).Property(x => x.IsActive).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index"/*, new { page = pageNumber }*/);
            }
            var validationErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.ValidationErrors = validationErrors;
            return View(model);
        }
        [Route("blog/Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.News.Find(id);
            if (item != null)
            {
                //var DeleteItem = db.Categories.Attach(item);
                db.News.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [Route("blog/IsActive")]
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.News.Find(id);
            if (item != null)
            {
                item.ModifiedDate = DateTime.Now;
                item.IsActive = !item.IsActive;

                db.Entry(item).Property(x => x.ModifiedDate).IsModified = true;
                db.Entry(item).Property(x => x.IsActive).IsModified = true;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }

            return Json(new { success = false });
        }
        [Route("blog/deleteAll")]
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
                        var obj = db.News.Find(Convert.ToInt32(item));
                        db.News.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
