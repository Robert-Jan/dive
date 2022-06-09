using System;
using Markdig;
using Dive.App.Models;
using System.Text.RegularExpressions;

namespace Dive.App.ViewModels
{
    public class PostViewModel : BaseViewModel
    {
        public Post Post;

        public string Timestamp => TimeZoneInfo.ConvertTime(Post.CreatedAt, Timezone).ToString("dd-MM-yyyy HH:mm");

        public string GetMarkdown()
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseSoftlineBreakAsHardlineBreak()
                .Build();

            return Markdown.ToHtml(Post.Body ?? "", pipeline);
        }

        public string Summary()
        {
            // Remove HTML elements
            var raw = Regex.Replace(GetMarkdown(), "<.*?>|&.*?;", string.Empty);

            // Remove whitespaces
            raw = raw.Replace("\n", "").Replace("\r", "");

            return !String.IsNullOrWhiteSpace(raw) && raw.Length > 200
                ? raw[..200] + "..."
                : raw;
        }
    }
}