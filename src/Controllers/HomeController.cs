using Dive.Data;
using Dive.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Dive.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DiveContext _context;

        public HomeController(ILogger<HomeController> logger, DiveContext context)
        {
            _logger = logger;
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
