using Dive.App.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dive.App.Repositories;
using System.Threading.Tasks;
using Dive.App.Models;
using Dive.App.Data;

namespace Dive.App.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPostRepository _postRepository;

        public HomeController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index(int page = 1, string filter = "newest")
        {
            PagedResult<Post> posts = filter switch
            {
                "newest" => await _postRepository.GetNewestPostsAsync(page),
                "unanswered" => await _postRepository.GetUnansweredPostsAsync(page),
                "activity" => await _postRepository.GetPostsWithRecentActivityAsync(page),
                _ => await _postRepository.GetNewestPostsAsync(page)
            };

            return View(new HomeViewModel
            {
                CurrentFilter = filter,
                Posts = posts
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
