using FoodShop_SWP.Models;
using FoodShop_SWP.Models.EF;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System;
using Microsoft.EntityFrameworkCore;

namespace FoodShop_SWP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ShopFoodWebContext context;

        public HomeController(ILogger<HomeController> logger, ShopFoodWebContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            List<News> posts = context.News.Take(3).ToList();
            ViewBag.posts = posts;
            return View();
        }
        [Route("/ForgotPassword")]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View("~/Views/User/ForgetPassword.cshtml");
        }

        public IActionResult ResetPassword(string email)
        {
            User user = context.Users.SingleOrDefault(u => u.Email == email);
            if(user == null)
            {
                ViewBag.error = "Email not existed!";
                return ForgotPassword();
            }
            else
            {
                string password = RandomString(8);
                user.Password = password;
                context.SaveChanges();
                SendMail(email, "Reset Password", "New Password is : " + password);
            }
            return RedirectToAction("Login", "User");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static void SendMail(string toAddress, string subject, string body)
        {
            try
            {
                var fromAddress = new MailAddress("bearlingo98@gmail.com");
                var receiver = new MailAddress(toAddress);
                const string fromPass = "njec jxuo moyv pjbg";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPass),
                    Timeout = 200000
                };
                using (var message = new MailMessage(fromAddress, receiver)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private  string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public IActionResult addtofavourite(int id)
        {
            string email = HttpContext.Session.GetString("Email");
            if(email == null)
            {
                ViewBag.error = "You need login first";
                return View("~/User/Login.cshtml");
            }
            else
            {
                User user = context.Users.SingleOrDefault(n => n.Email == email);
                Product product = context.Products.SingleOrDefault(n => n.Id == id);
                Favourite fa= new Favourite()
                {
                    user = user,
                    product = product,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                context.Add(fa);
                context.SaveChanges();
                string url = Request.Headers["Referer"];
                return RedirectToPage(url);
            }
        }
    }
}