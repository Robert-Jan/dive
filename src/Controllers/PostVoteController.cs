using System.Threading.Tasks;
using Dive.App.Models;
using Dive.App.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dive.App.Controllers
{
    public class PostVoteController : BaseController
    {
        private readonly IUserRepository _userRepository;

        private readonly IPostRepository _postRepository;

        private readonly IVoteRepository _voteRepository;

        public PostVoteController(
            IPostRepository postRepository,
            IUserRepository userRepository,
            IVoteRepository voteRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _voteRepository = voteRepository;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpGet("/questions/{id}/vote/{type}")]
        public async Task<IActionResult> Vote(int id, VoteType type)
        {
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null) return NotFound();

            User user = await _userRepository.GetCurrentUserAsync();
            var vote = _voteRepository.SetVoteAsync(post, user, type);
            var score = _voteRepository.GetVoteScoreAsync(post);

            await _postRepository.UpdateVoteScoreAsync(post, score);

            return Json(new { score, vote });
        }
    }
}