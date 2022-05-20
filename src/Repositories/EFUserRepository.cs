using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

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
    }
}