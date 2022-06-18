using System;
using Markdig;
using Dive.App.Models;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Dive.App.ViewModels
{
    public class PostViewModel : BaseViewModel
    {
        public Post Post;

        public List<Vote> GivenVotes = new();

        public string Timestamp => GetTimestamp(Post.CreatedAt);

        public int GetGivenVote()
        {
            var vote = GivenVotes.Find(v => v.PostId == Post.Id);
            return (int)(vote?.Type ?? 0);
        }

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