using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;

namespace Dive.App.Repositories
{
    public interface IPostRepository
    {
        Task<PagedResult<Post>> GetNewestPostsAsync(int page = 1);

        Task<PagedResult<Post>> GetUnansweredPostsAsync(int page = 1);

        Task<PagedResult<Post>> GetPostsWithRecentActivityAsync(int page = 1);

        Task<int> StorePostAsync(Post post, User user);

        Task<int> StoreTagsOnPostAsync(Post post, string[] tags);
    }
}