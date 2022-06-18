using System.Collections.Generic;
using System.Threading.Tasks;
using Dive.App.Models;

namespace Dive.App.Repositories
{
    public interface IVoteRepository
    {
        Task<List<Vote>> GetGivenVotes(Post post, User user);

        int GetVoteScoreAsync(Post post);

        object SetVoteAsync(Post post, User user, VoteType type);
    }
}