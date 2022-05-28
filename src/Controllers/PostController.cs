using System.Linq;
using System.Threading.Tasks;
using Dive.App.Models;
using Dive.App.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dive.App.Controllers
{
    public class PostController : BaseController
    {
        private readonly IUserRepository _userRepository;

        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        [Authorize]
        [HttpGet("/ask")]
        public IActionResult New()
        {
            return View();
        }

        [Authorize]
        [HttpPost("/ask")]
        public async Task<IActionResult> New([Bind("Title,Body")] Post post, string[] tags)
        {
            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetCurrentUserAsync();
                await _postRepository.StorePostAsync(post, user);
                await _postRepository.StoreTagsOnPostAsync(post, tags.Take(1).ToArray());

                SetNotification("Success", "Your question is successfully created");

                return Redirect("/");
            }

            return View();
        }
    }
}
