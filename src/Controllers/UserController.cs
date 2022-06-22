using Dive.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Dive.App.Repositories;
using System.Threading.Tasks;
using Dive.App.Models;
using Dive.App.Data;

namespace Dive.App.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRepository _UserRepository;

        private readonly IPostRepository _postRepository;

        public UserController(IUserRepository userRepository, IPostRepository postRepository)
        {
            _UserRepository = userRepository;
            _postRepository = postRepository;
        }

        [HttpGet("/users")]
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(new UserIndexViewModel
            {
                Users = await _UserRepository.GetUsersAsync(page)
            });
        }

        [HttpGet("/users/{id}")]
        public async Task<IActionResult> Detail(int id, int page = 1)
        {
            var user = await _UserRepository.GetUserByIdAsync(id);
            var posts = await _postRepository.GetPostsFromUserAsync(user);

            return View(new UserDetailViewModel { User = user, Posts = posts });
        }
    }
}
