using System.Threading.Tasks;
using System.Linq;
using Dive.App.Data;
using Dive.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Dive.App.Repositories
{
    public class EFSearchRepository : ISearchRepository
    {
        private readonly DiveContext _context;

        public EFSearchRepository(DiveContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Post>> GetPagedResultAsync(string query, int page = 1)
        {
            var tag = await _context.Tags
                .Where(t => t.Name.Contains(query))
                .FirstOrDefaultAsync();

            return await _context.Posts
                .Where(p => p.ParentId == null)
                .Where(p => p.SearchVector.Matches(query) || p.Tags.Contains(tag))
                .Include(p => p.Tags)
                .Include(p => p.User)
                .GetPagedAsync(page, 10);
        }
    }
}