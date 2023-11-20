using Microsoft.AspNetCore.Mvc;

namespace FoodShop_SWP.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class HomeController : Controller
    {
        [Route("")]
        [Route("home")]
        public IActionResult Index()
        {
            return RedirectToAction("Index","Statistical");
        }
    }
}
