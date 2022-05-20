using System.Threading.Tasks;
using Dive.App.Models;

namespace Dive.App.Repositories
{
    public interface IUserRepository
    {
        User GetCurrentUser();
        
        Task<User> GetCurrentUserAsync();
    }
}