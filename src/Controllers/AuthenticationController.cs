using Dive.App.Models;
using Dive.App.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Dive.App.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("/login")]
        public IActionResult Login(string ReturnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = ReturnUrl });
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(string username, string password, bool remember, string returnUrl)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, remember, false);

                if (signInResult.Succeeded)
                {
                    return returnUrl == "" || returnUrl == null
                        ? RedirectToAction(nameof(HomeController.Index), "Home")
                        : Redirect(returnUrl);
                }
            }

            return View(new LoginViewModel { LoginFailed = true });
        }

        [Authorize]
        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(string email, string username, string password, string first_name, string last_name)
        {
            var user = new User
            {
                UserName = username,
                Email = email,
                FirstName = first_name,
                LastName = last_name
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded) return RedirectToAction(nameof(Login));

            return RedirectToAction(nameof(Register));
        }
    }
}