using FoodShop_SWP.Models;
using FoodShop_SWP.Models.EF;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop_SWP.Controllers
{
    public class FeedBackController : Controller
    {
        private readonly ShopFoodWebContext _context;

        public FeedBackController(ShopFoodWebContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult addComment(int productID, string comment)
        {
            string email = HttpContext.Session.GetString("Email");
            if(email != null)
            {
                User user = _context.Users.SingleOrDefault(n => n.Email == email);
                Product product = _context.Products.SingleOrDefault(n => n.Id == productID);
                Feedback feedback = new Feedback()
                {
                    ProductID = productID,
                    product = product,
                    Content = comment,
                    user = user,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                _context.Add(feedback);
                _context.SaveChanges();
            }
            return Redirect("/Product/ProductDetail?id=" + productID);
        }
    }
}
