using Dive.App.Data;
using Dive.App.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Dive.App.Models;
using Microsoft.AspNetCore.Identity;

namespace Dive.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly DiveContext _context;

        private readonly UserManager<User> _userManager;

        public HomeController(DiveContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            return View(new HomeViewModel
            {
                UserData = await _userManager.GetUserAsync(HttpContext.User),
                Posts = await _context.Posts.ToListAsync()
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
