using System;
using Dive.App.Models;
using System.Text.RegularExpressions;

namespace Dive.App.ViewModels
{
    public class PostViewModel : BaseViewModel
    {
        public Post Post;

        public string Timestamp => TimeZoneInfo.ConvertTime(Post.CreatedAt, Timezone).ToString("dd-MM-yyyy HH:mm");

        public string Summary()
        {
            string content = Post.Body;

            // Remove markdown elements
            content = Regex.Replace(content, "/\n={2,}/g", "\n");
            content = Regex.Replace(content, "/~~/g", string.Empty);
            content = Regex.Replace(content, "/`{3}.*\n/g", string.Empty);
            content = Regex.Replace(content, "/<[^>]*>/g", string.Empty);
            content = Regex.Replace(content, "/^[=\\-]{2,}\\s*$/g", string.Empty);
            content = Regex.Replace(content, "/\\[\\^.+?\\](\\: .*?$)?/g", string.Empty);
            content = Regex.Replace(content, "/\\s{0,2}\\[.*?\\]: .*?$/g", string.Empty);
            content = Regex.Replace(content, "/\\!\\[.*?\\][\\[\\(].*?[\\]\\)]/g", string.Empty);
            content = Regex.Replace(content, "/\\[(.*?)\\][\\[\\(].*?[\\]\\)]/g", "$1");

            return !String.IsNullOrWhiteSpace(content) && content.Length > 200
                ? content[..200] + "..."
                : content;
        }
    }
}