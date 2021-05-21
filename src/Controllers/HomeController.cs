using Dive.App.Data;
using Dive.App.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Dive.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly DiveContext _context;

        public HomeController(DiveContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Boards.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
