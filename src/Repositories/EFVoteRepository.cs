using Dive.App.Data;
using Dive.App.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dive.App.Repositories
{
    public class EFVoteRepository : IVoteRepository
    {
        private readonly DiveContext _context;

        public EFVoteRepository(DiveContext context)
        {
            _context = context;
        }

        public Task<List<Vote>> GetGivenVotes(Post post, User user)
        {
            List<int> ids = post.Anwsers.Select(a => a.Id).ToList();
            ids.Add(post.Id);

            return _context.Votes
                .Where(v => ids.Contains(v.PostId))
                .Where(v => v.UserId == user.Id)
                .ToListAsync();
        }

        public int GetVoteScoreAsync(Post post)
        {
            int score = 0;
            var votes = _context.Votes
                .Where(v => v.PostId == post.Id)
                .ToList();

            votes.ForEach(v =>
            {
                score = v.Type == VoteType.Upvote ? score + 1 : score - 1;
            });

            return score;
        }

        public object SetVoteAsync(Post post, User user, VoteType type)
        {
            var vote = _context.Votes
                .Where(v => v.PostId == post.Id && v.UserId == user.Id)
                .FirstOrDefault();

            if (vote != null)
            {
                _context.Remove(vote);
                _context.SaveChanges();

                // Return if the given vote was the same so it is only a remove.
                if (vote.Type == type) return null;
            }

            vote = new Vote
            {
                Post = post,
                User = user,
                Type = type
            };

            _context.Votes.Add(vote);
            _context.SaveChanges();

            return vote.Type;
        }
    }
}