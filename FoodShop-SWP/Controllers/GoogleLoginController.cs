using FoodShop_SWP.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodShop_SWP.Controllers
{
    public class GoogleLoginController : Controller
    {
        private readonly ShopFoodWebContext context;

        public GoogleLoginController(ShopFoodWebContext context)
        {
            this.context = context;
        }

        public IActionResult OnPost()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback", "GoogleLogin"),
            };
            return Challenge(properties, "Google");
        }

        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync("Google");

            if (result.Succeeded)
            {
                var emailLogin = result?.Principal?.FindFirst(ClaimTypes.Email)?.Value;
                var user = context.Users.FirstOrDefault(x => x.Email == emailLogin);
                if (user == null)
                {
                    ViewBag.Email = emailLogin;
                    ViewBag.Mess = "Bạn cần đăng kí tài khoản trước khi sử dụng dịch vụ";
                    return View("~/Views/User/Register.cshtml");
                }
                else
                {
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (result?.Properties?.Items.ContainsKey("error") == true)
            {
                var error = result.Properties.Items["error"];
                var errorDescription = result.Properties.Items["error_description"];
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
