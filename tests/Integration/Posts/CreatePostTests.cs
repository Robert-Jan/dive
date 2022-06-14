using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dive.App.Data;
using Xunit;

namespace Dive.Tests.Integration.Posts
{
    public class CreatePostTests : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Guest { get; }

        public HttpClient LoggedIn { get; }

        public DiveContext Context { get; }

        public CreatePostTests(TestApplicationFixture app)
        {
            Guest = app.CreateClient();
            LoggedIn = app.CreateAuthenticatedClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task a_user_can_create_a_new_post()
        {
            var response = await LoggedIn.PostAsync("/ask", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("title", "Example title"),
                new KeyValuePair<string, string>("body", "Example body"),
            }));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var post = Context.Posts.OrderByDescending(p => p.Id).First();
            Assert.Equal("/questions/" + post.Id, TestUtilities.ResponseUrl(response));
            Assert.Equal("Example title", post.Title);
            Assert.Equal("Example body", post.Body);
        }

        [Fact]
        public async Task a_guest_cannot_create_a_post()
        {
            var response = await Guest.PostAsync("/ask", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("title", "Example title"),
                new KeyValuePair<string, string>("body", "Example body"),
            }));

            Assert.Equal("/login?ReturnUrl=%2Fask", TestUtilities.ResponseUrl(response));
        }
    }
}
