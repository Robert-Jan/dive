using System.Threading.Tasks;
using Dive.App.Models;

namespace Dive.App.Repositories
{
    public interface ITagRepository
    {
        Task<Tag> GetByTagOrCreateAsync(string tag);
    }
}