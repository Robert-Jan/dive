using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;

namespace Dive.App.Repositories
{
    public interface ISearchRepository
    {
        Task<PagedResult<Post>> GetPagedResultAsync(string query, int page = 1);
    }
}