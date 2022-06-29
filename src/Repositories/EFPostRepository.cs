using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Dive.App.Repositories
{
    public class EFPostRepository : IPostRepository
    {
        private readonly DiveContext _context;

        private readonly ITagRepository _tagRepository;

        public EFPostRepository(DiveContext context, ITagRepository tagRepository)
        {
            _context = context;
            _tagRepository = tagRepository;
        }

        public Task<PagedResult<Post>> GetNewestPostsAsync(int page = 1)
        {
            return _context.Posts
                .Where(p => p.ParentId == null)
                .Include(p => p.Tags)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .GetPagedAsync(page, 10);
        }

        public Task<PagedResult<Post>> GetUnansweredPostsAsync(int page = 1)
        {
            return _context.Posts
                .Where(p => p.ParentId == null)
                .Where(p => p.AcceptedAnswerId == null)
                .Include(p => p.Tags)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .GetPagedAsync(page, 10);
        }

        public Task<PagedResult<Post>> GetPostsWithRecentActivityAsync(int page = 1)
        {
            return _context.Posts
                .Where(p => p.ParentId == null)
                .Include(p => p.Tags)
                .Include(p => p.User)
                .OrderByDescending(p => p.UpdatedAt)
                .GetPagedAsync(page, 10);
        }

        public Task<PagedResult<Post>> GetPostsFromUserAsync(User user, int page = 1)
        {
            return _context.Posts
                .Where(p => p.UserId == user.Id)
                .Where(p => p.ParentId == null)
                .Include(p => p.Tags)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .GetPagedAsync(page, 10);
        }

        public Task<Post> GetByIdAsync(int id)
        {
            return _context.Posts
                .Include(p => p.User)
                .Where(p => p.Id == id)
                .FirstAsync();
        }

        public Task<Post> GetPostDetailsAsync(int id)
        {
            return _context.Posts
                .Where(p => p.ParentId == null)
                .Where(p => p.Id == id)
                .Include(p => p.User)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                    .ThenInclude(comments => comments.User)
                .Include(p => p.Anwsers)
                    .ThenInclude(anwsers => anwsers.User)
                .Include(p => p.Anwsers)
                    .ThenInclude(anwsers => anwsers.Comments)
                        .ThenInclude(anwserComments => anwserComments.User)
                .AsSplitQuery()
                .FirstAsync();
        }

        public List<Post> GetRelatedPosts(Post post, int amount = 10)
        {
            List<Post> posts = new();

            post.Tags.ToList().ForEach(tag =>
            {
                var result = _context.Posts
                    .Where(p => p.Id != post.Id)
                    .Where(p => p.ParentId == null)
                    .Where(p => p.Tags.Contains(tag))
                    .Include(p => p.Tags)
                    .OrderByDescending(p => p.VoteScore)
                    .ThenBy(p => p.ViewsCount)
                    .Take(amount * 5)
                    .ToList();

                result.ForEach(p => posts.Add(p));
            });

            return posts.GroupBy(p => p.Id)
                .Select(p => new { Post = p.First(), Count = p.Count() })
                .OrderByDescending(p => p.Count)
                .Take(amount)
                .Select(x => x.Post)
                .ToList();
        }

        public Task<int> StorePostAsync(Post post, User user)
        {
            post.Type = (PostType)(int)PostType.Question;
            post.UserId = user.Id;
            _context.Posts.Add(post);

            return _context.SaveChangesAsync();
        }

        public async Task<int> StoreTagsOnPostAsync(Post post, string[] tags)
        {
            foreach (var tag in tags)
            {
                var tagEntity = await _tagRepository.GetByTagOrCreateAsync(tag);

                post.Tags.Add(tagEntity);
            }

            return await _context.SaveChangesAsync();
        }

        public Task<int> StoreAnwserAsync(Post post, Post anwser, User user)
        {
            anwser.Type = (PostType)(int)PostType.Answer;
            anwser.UserId = user.Id;
            anwser.ParentId = post.Id;
            _context.Posts.Add(anwser);

            return _context.SaveChangesAsync();
        }

        public Task<int> SetAcceptedAnswerAsync(Post post, Post anwser)
        {
            post.AcceptedAnswer = anwser;

            return _context.SaveChangesAsync();
        }

        public Task<int> UpdateVoteScoreAsync(Post post, int score)
        {
            post.VoteScore = score;

            return _context.SaveChangesAsync();
        }
        public Task<int> UpdateTimestampAsync(Post post)
        {
            post.UpdateTimestamp();

            return _context.SaveChangesAsync();
        }

        public Task<int> SyncCountersAsync(Post post)
        {
            post.AnwsersCount = _context.Posts.Count(p => p.ParentId == post.Id);
            post.ViewsCount = _context.Views.Count(v => v.PostId == post.Id);

            return _context.SaveChangesAsync();
        }

        public async Task<int> RegisterViewAsync(Post post, User user)
        {
            var view = _context.Views
                .Where(v => v.PostId == post.Id && v.UserId == user.Id)
                .FirstOrDefault();

            if (view == null) _context.Views.Add(new View { PostId = post.Id, UserId = user.Id });
            else view.UpdateTimestamp();

            await _context.SaveChangesAsync();

            return await SyncCountersAsync(post);
        }
    }
}