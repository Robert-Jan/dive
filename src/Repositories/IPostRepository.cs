using System.Collections.Generic;
using System.Threading.Tasks;
using Dive.App.Models;

namespace Dive.App.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetNewestPostsAsync();

        Task<int> StorePostAsync(Post post, User user);

        Task<int> StoreTagsOnPostAsync(Post post, string[] tags);
    }
}