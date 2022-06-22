using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using Microsoft.AspNetCore.Identity;

namespace Dive.App.Repositories
{
    public interface IUserRepository
    {
        User GetCurrentUser();

        Task<User> GetCurrentUserAsync();

        Task<int> UpdateUserAsync(int id, string email, string first_name, string last_name);

        Task<IdentityResult> UpdatePasswordAsync(User user, string currentPassword, string newPassword);

        Task<int> SyncCountersAsync(User user);

        Task<User> GetUserByIdAsync(int id);

        Task<PagedResult<User>> GetUsersAsync(int page = 1);
    }
}