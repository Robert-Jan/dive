using System.Collections.Generic;
using Dive.App.Data;
using Dive.App.Models;

namespace Dive.App.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public List<KeyValuePair<string, string>> Filters = new()
        {
            new KeyValuePair<string, string>("newest", "Newest"),
            new KeyValuePair<string, string>("unanswered", "Unanswered"),
            new KeyValuePair<string, string>("activity", "Recent activity")
        };

        public string CurrentFilter;

        public PagedResult<Post> Posts;
    }
}
