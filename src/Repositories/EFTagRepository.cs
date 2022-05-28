using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Dive.App.Repositories
{
    public class EFTagRepository : ITagRepository
    {
        private readonly DiveContext _context;

        public EFTagRepository(DiveContext context)
        {
            _context = context;
        }

        public async Task<Tag> GetByTagOrCreateAsync(string tag)
        {
            var tagEntity = await _context.Tags.SingleOrDefaultAsync(t => t.Name == tag);

            if (tagEntity == null)
            {
                tagEntity = new Tag { Name = tag };
                _context.Tags.Add(tagEntity);

                await _context.SaveChangesAsync();
            }

            return await Task.FromResult(tagEntity);
        }
    }
}