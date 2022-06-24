using System.Linq;
using System.Threading.Tasks;
using Dive.App.Models;
using Dive.App.Repositories;
using Dive.App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dive.App.Controllers
{
    public class PostController : BaseController
    {
        private readonly IUserRepository _userRepository;

        private readonly IPostRepository _postRepository;

        private readonly IVoteRepository _voteRepository;

        public PostController(
            IPostRepository postRepository,
            IUserRepository userRepository,
            IVoteRepository voteRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _voteRepository = voteRepository;
        }

        [HttpGet("/questions/{id}")]
        public async Task<IActionResult> Post(int id)
        {
            var post = await _postRepository.GetPostDetailsAsync(id);

            if (post == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userRepository.GetCurrentUserAsync();

                var votes = await _voteRepository.GetGivenVotes(post, user);
                await _postRepository.RegisterViewAsync(post, user);

                return View(new PostViewModel { Post = post, GivenVotes = votes });
            }
            else return View(new PostViewModel { Post = post });
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
                await _postRepository.StoreTagsOnPostAsync(post, tags.Take(5).ToArray());

                await _userRepository.SyncCountersAsync(user);
                SetNotification("Success", "Your question is successfully created");

                return RedirectToAction(nameof(Post), new { id = post.Id });
            }

            return View();
        }

        [Authorize]
        [HttpPost("/questions/{id}/anwser")]
        public async Task<IActionResult> Anwser(int id, [Bind("Body")] Post anwser)
        {
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null || post.ParentId != null) return NotFound();

            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetCurrentUserAsync();
                await _postRepository.StoreAnwserAsync(post, anwser, user);
                await _postRepository.SyncCountersAsync(post);

                SetNotification("Success", "Your anwser is successfully added");
            }

            return RedirectToAction(nameof(Post), new { id = post.Id });
        }

        [Authorize]
        [HttpGet("/questions/{id}/accept/{anwserId}")]
        public async Task<IActionResult> Accept(int id, int anwserId)
        {
            var post = await _postRepository.GetPostDetailsAsync(id);
            var anwser = await _postRepository.GetByIdAsync(anwserId);
            User user = await _userRepository.GetCurrentUserAsync();

            if (anwser.ParentId != post.Id || post.UserId != user.Id) return NotFound();

            await _postRepository.SetAcceptedAnswerAsync(post, anwser);

            foreach (Post a in post.Anwsers) await _userRepository.SyncCountersAsync(a.User);

            return RedirectToAction(nameof(Post), new { id = post.Id });
        }
    }
}
