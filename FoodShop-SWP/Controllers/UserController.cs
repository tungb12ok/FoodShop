using FoodShop_SWP.Models;
using FoodShop_SWP.Models.EF;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop_SWP.Controllers
{
    public class UserController : Controller
    {
        ShopFoodWebContext db = new ShopFoodWebContext();
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                ViewBag.Mess = "";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                var u = db.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("Email", u.Email.ToString());
                    HttpContext.Session.SetString("UserId", u.Id.ToString());

                    // role = 1 là customer role bằng 2 sẽ là admin 
                    if (u.Role == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect("/admin/statistical");
                    }
                }
                else
                {
                    ViewBag.Email = email;
                    ViewBag.Password = password;
                    ViewBag.Mess = "Email or password incorrect. Try again!";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "User");
        }
        [HttpGet]

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string email, string password, string fname, string lname, string gender)
        {
            try
            {
                var user = db.Users.Where(u => u.Email == email).FirstOrDefault();
                ViewBag.fname = fname;
                ViewBag.lname = lname;
                ViewBag.gender = gender;
                ViewBag.Email = email;
                ViewBag.password = password;
                if (user != null)
                {
                    ViewBag.Mess = "This Email already exist!";
                    return View();
                }
                else
                {
                    User u = new User();
                    u.Email = email;
                    u.Password = password;
                    u.Firrtname = fname;
                    u.Lastname = lname;
                    u.Gender = gender.Equals("1");
                    u.Enable = true;
                    u.IsLockout = false;
                    u.Role = 1;
                    u.ImageUrl = "https://static.vecteezy.com/system/resources/thumbnails/001/840/618/small/picture-profile-icon-male-icon-human-or-people-sign-and-symbol-free-vector.jpg";
                    db.Users.Add(u);
                    db.SaveChanges();
                    return RedirectToAction("Login", "User", new { RegisterMess = "Register new account successful!" });

                }
            }
            catch (Exception e)
            {
                ViewBag.Mess = "Register failed!";
                return View();
            }
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(string password, string newpass, string renewpass)
        {
            string email = HttpContext.Session.GetString("Email").ToString();
            var user = db.Users.Where(u => u.Email == email).FirstOrDefault();
            if (user.Password != password)
            {
                ViewBag.Mess = "Old password incorrect!";
                return View();
            }
            if (newpass != renewpass)
            {
                ViewBag.Mess = "New password not match with  confirm password";
                return View();
            }
            user.Password = newpass;
            db.Users.Update(user);
            db.SaveChanges();
            ViewBag.MessSuc = "Change password successfull!";
            return View();
        }
    }
}
