using Dive.App.Data;
using Dive.App.Models;

namespace Dive.App.ViewModels
{
    public class UserIndexViewModel : BaseViewModel
    {
        public PagedResult<User> Users;
    }
}
