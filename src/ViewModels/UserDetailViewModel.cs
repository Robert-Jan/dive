using Dive.App.Data;
using Dive.App.Models;

namespace Dive.App.ViewModels
{
    public class UserDetailViewModel : BaseViewModel
    {
        public User User;

        public PagedResult<Post> Posts;
    }
}
