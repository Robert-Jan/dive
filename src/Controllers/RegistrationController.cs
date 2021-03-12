using Dive.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Dive.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly UserManager<User> _userManager;

        public RegistrationController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(string email, string username, string password)
        {
            var user = new User
            {
                UserName = username,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Authentication");
            }

            return RedirectToAction("Login", "Authentication");
        }
    }
}