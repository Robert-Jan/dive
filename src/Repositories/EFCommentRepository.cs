using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;

namespace Dive.App.Repositories
{
    public class EFCommentRepository : ICommentRepository
    {
        private readonly DiveContext _context;

        public EFCommentRepository(DiveContext context)
        {
            _context = context;
        }

        public async Task<int> StoreCommentOnPostAsync(Post post, Comment comment, User user)
        {
            comment.User = user;
            comment.Post = post;

            _context.Comments.Add(comment);

            return await _context.SaveChangesAsync();
        }
    }
}