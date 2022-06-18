using System;
using System.Collections.Generic;
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

            var votes = User.Identity.IsAuthenticated
                ? await _voteRepository.GetGivenVotes(post, await _userRepository.GetCurrentUserAsync())
                : new List<Vote>();

            return View(new PostViewModel
            {
                Post = post,
                GivenVotes = votes
            });
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

                SetNotification("Success", "Your anwser is successfully added");
            }

            return RedirectToAction(nameof(Post), new { id = post.Id });
        }
    }
}
