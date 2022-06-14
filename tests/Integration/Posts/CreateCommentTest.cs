using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using Xunit;

namespace Dive.Tests.Integration.Posts
{
    public class CreateCommentTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Guest { get; }

        public HttpClient LoggedIn { get; }

        public DiveContext Context { get; }

        public CreateCommentTest(TestApplicationFixture app)
        {
            Guest = app.CreateClient();
            LoggedIn = app.CreateAuthenticatedClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task a_user_can_add_an_comment_to_a_post()
        {
            var post = new Post { Title = "Example title", Body = "Example body", UserId = Context.Users.First().Id };
            Context.Posts.Add(post);
            Context.SaveChanges();

            var response = await LoggedIn.PostAsync($"/questions/{post.Id}/comments", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("body", "Example comment"),
            }));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var comment = Context.Comments.OrderByDescending(p => p.Id).First();
            Assert.Equal("/questions/" + post.Id, TestUtilities.ResponseUrl(response));
            Assert.Equal(post, comment.Post);
            Assert.Equal("Example comment", comment.Body);
        }

        [Fact]
        public async Task a_guest_cannot_create_a_comment()
        {
            var post = new Post { Title = "Example title", Body = "Example body", UserId = Context.Users.First().Id };
            Context.Posts.Add(post);
            Context.SaveChanges();

            var response = await Guest.PostAsync($"/questions/{post.Id}/comments", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("body", "Example comment"),
            }));

            Assert.Equal($"/login?ReturnUrl=%2Fquestions%2F{post.Id}%2Fcomments", TestUtilities.ResponseUrl(response));
        }
    }
}
