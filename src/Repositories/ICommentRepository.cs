using System.Threading.Tasks;
using Dive.App.Models;

namespace Dive.App.Repositories
{
    public interface ICommentRepository
    {
        Task<int> StoreCommentOnPostAsync(Post post, Comment comment, User user);
    }
}