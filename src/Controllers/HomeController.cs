using Dive.App.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Dive.App.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
