using Microsoft.AspNetCore.Mvc;
using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Threading.Tasks;

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

            var result = await _signInManager.PasswordSignInAsync(userLoginData.UserName, userLoginData.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Events", "Events");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your username and password.");
                return View(userLoginData);
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register userRegisterData)
        {
            if (!ModelState.IsValid)
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
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }

            // Handle failures, e.g., display an error message
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> EditUsername(string newUsername)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.UserName = newUsername;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    return RedirectToAction("Profile");
                }
            }
            // Handle errors appropriately
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(string currentPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    return RedirectToAction("Profile");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
            }

            return View("Profile", user);
        }


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }
    }
}
