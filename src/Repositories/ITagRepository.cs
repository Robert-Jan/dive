using System.Collections.Generic;
using System.Threading.Tasks;
using Dive.App.Models;

namespace Dive.App.Repositories
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAllTagsAsync();

        Task<Tag> GetByTagOrCreateAsync(string tag);
    }
}