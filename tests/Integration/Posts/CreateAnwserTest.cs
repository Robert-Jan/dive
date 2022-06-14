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
    public class CreateAnwserTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Guest { get; }

        public HttpClient LoggedIn { get; }

        public DiveContext Context { get; }

        public CreateAnwserTest(TestApplicationFixture app)
        {
            Guest = app.CreateClient();
            LoggedIn = app.CreateAuthenticatedClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task a_user_can_add_an_anwser_to_a_post()
        {
            var post = new Post { Title = "Example title", Body = "Example body", UserId = Context.Users.First().Id };
            Context.Posts.Add(post);
            Context.SaveChanges();

            var response = await LoggedIn.PostAsync($"/questions/{post.Id}/anwser", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("body", "Example answer"),
            }));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var anwser = Context.Posts.OrderByDescending(p => p.Id).First();
            Assert.Equal("/questions/" + post.Id, TestUtilities.ResponseUrl(response));
            Assert.Equal(post, anwser.Parent);
            Assert.Equal("Example answer", anwser.Body);
        }

        [Fact]
        public async Task a_guest_cannot_create_a_anwser()
        {
            var post = new Post { Title = "Example title", Body = "Example body", UserId = Context.Users.First().Id };
            Context.Posts.Add(post);
            Context.SaveChanges();

            var response = await Guest.PostAsync($"/questions/{post.Id}/anwser", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("body", "Example answer"),
            }));

            Assert.Equal($"/login?ReturnUrl=%2Fquestions%2F{post.Id}%2Fanwser", TestUtilities.ResponseUrl(response));
        }
    }
}
