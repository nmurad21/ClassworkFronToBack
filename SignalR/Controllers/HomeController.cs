using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalR.Models;
using SignalR.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;


        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
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
        public IActionResult Chat()
        {
            List<AppUser> users = _userManager.Users.ToList();
            return View(users);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser dbUser = _userManager.FindByNameAsync(login.Username).Result;
            if (dbUser==null)
            {
                ModelState.AddModelError("", "username veya parol duzgun daxil edilmeyib");
            }
            Microsoft.AspNetCore.Identity.SignInResult result = _signInManager.PasswordSignInAsync(dbUser, login.Password, true, true).Result;
            if (result.Succeeded)
            {
                ModelState.AddModelError("", "username veya parol duzgun daxil edilmeyib");
            }
            await _signInManager.SignInAsync(dbUser, true);

            return RedirectToAction("chat", "home");
        }

        public IActionResult Register()
        {
            AppUser user1 = new AppUser { UserName = "_ilkin", Fullname = "ilkin" };
            AppUser user2 = new AppUser { UserName = "_murad", Fullname = "Murad" };
            AppUser user3 = new AppUser { UserName = "_resid", Fullname = "Resid" };
            AppUser user4 = new AppUser { UserName = "_yusif", Fullname = "Yusif" };

           var result1= _userManager.CreateAsync(user1, "User@123").Result;
            var result2 = _userManager.CreateAsync(user2, "User@123").Result;
            var result3 = _userManager.CreateAsync(user3, "User@123").Result;
            var result4 = _userManager.CreateAsync(user4, "User@123").Result;

            return Content("Userler created");

        } 
        public async Task<IActionResult> Logout()
        {
          await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
    }
}
