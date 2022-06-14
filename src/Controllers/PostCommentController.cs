using System.Threading.Tasks;
using Dive.App.Models;
using Dive.App.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dive.App.Controllers
{
    public class PostCommentController : BaseController
    {
        private readonly IUserRepository _userRepository;

        private readonly IPostRepository _postRepository;

        private readonly ICommentRepository _commentRepository;

        public PostCommentController(
            IPostRepository postRepository,
            IUserRepository userRepository,
            ICommentRepository commentRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        [Authorize]
        [HttpPost("/questions/{id}/comments")]
        public async Task<IActionResult> Anwser(int id, [Bind("Body")] Comment comment)
        {
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null) return NotFound();

            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetCurrentUserAsync();
                await _commentRepository.StoreCommentOnPostAsync(post, comment, user);

                SetNotification("Success", "Your comment is successfully added");
            }

            var returnPostId = post.ParentId ?? post.Id;

            return RedirectToAction(nameof(PostController.Post), "Post", new { id = returnPostId });
        }
    }
}
