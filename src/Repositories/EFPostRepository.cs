using System.Collections.Generic;
using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        public Task<List<Post>> GetNewestPostsAsync()
        {
            return _context.Posts
                .Where(p => p.ParentId == null)
                .Include(p => p.Tags)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
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
    }
}