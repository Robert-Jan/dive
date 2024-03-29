using System.Collections.Generic;
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

        Task<PagedResult<Post>> GetPostsFromUserAsync(User user, int page = 1);

        Task<Post> GetByIdAsync(int id);

        Task<Post> GetPostDetailsAsync(int id);

        List<Post> GetRelatedPosts(Post post, int amount = 10);

        Task<int> StorePostAsync(Post post, User user);

        Task<int> StoreTagsOnPostAsync(Post post, string[] tags);

        Task<int> StoreAnwserAsync(Post post, Post anwser, User user);

        Task<int> SetAcceptedAnswerAsync(Post post, Post anwser);

        Task<int> UpdateTimestampAsync(Post post);

        Task<int> UpdateVoteScoreAsync(Post post, int score);

        Task<int> SyncCountersAsync(Post post);

        Task<int> RegisterViewAsync(Post post, User user);
    }
}