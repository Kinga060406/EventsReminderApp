using Microsoft.AspNetCore.Mvc;
using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Identity;

namespace EventsReminderApp.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login userLoginData)
        {
            if (!ModelState.IsValid)
            {
                return View(userLoginData);
            }

            await _signInManager.PasswordSignInAsync(userLoginData.UserName, userLoginData.Password, false, false);

            //przekierowanie do innego kontrolera
            return RedirectToAction("Events", "Events");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register userRegisterData)
        {
            if(!ModelState.IsValid)
            {
                return View(userRegisterData);
            }

            var newUser = new UserModel
            {
                Email = userRegisterData.Email,
                UserName = userRegisterData.UserName
            };

            await _userManager.CreateAsync(newUser, userRegisterData.Password);

            //przekierowanie do innego kontrolera
            return RedirectToAction("Events", "Events");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home", "Home");
        }
    }
}
