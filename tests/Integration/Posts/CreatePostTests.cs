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
        public HttpClient Client { get; }

        public DiveContext Context { get; }

        public CreatePostTests(TestApplicationFixture app)
        {
            Client = app.CreateAuthenticatedClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task a_user_can_create_a_new_post()
        {
            var response = await Client.PostAsync("/ask", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("title", "Example title"),
                new KeyValuePair<string, string>("body", "Example body"),
            }));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var post = Context.Posts.OrderByDescending(p => p.Id).First();
            Assert.Equal("Example title", post.Title);
            Assert.Equal("Example body", post.Body);
        }
    }
}
