using System.Threading.Tasks;
using System.Linq;
using Dive.App.Data;
using Dive.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dive.App.Repositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly DiveContext _context;

        private readonly UserManager<User> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public EFUserRepository(DiveContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public User GetCurrentUser()
        {
            var id = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

            return _context.Users.Find(int.Parse(id));
        }

        public Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }

        public Task<int> UpdateUserAsync(int id, string email, string firstName, string lastName)
        {
            var user = _context.Users.Where(u => u.Id == id).First();

            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;

            return _context.SaveChangesAsync();
        }

        public Task<IdentityResult> UpdatePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public Task<int> SyncCountersAsync(User user)
        {
            user.QuestionsCount = _context.Posts.Count(p => p.UserId == user.Id && p.ParentId == null);

            user.CorrectAnswersCount = _context.Posts
                .Include(p => p.Parent)
                .Where(p => p.Parent.AcceptedAnswerId == p.Id)
                .Where(p => p.UserId == user.Id)
                .Count();

            return _context.SaveChangesAsync();
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return _context.Users
                .Where(u => u.Id == id)
                .FirstAsync();
        }

        public Task<PagedResult<User>> GetUsersAsync(int page = 1)
        {
            return _context.Users
                .OrderByDescending(p => p.CreatedAt)
                .GetPagedAsync(page, 12);
        }
    }
}