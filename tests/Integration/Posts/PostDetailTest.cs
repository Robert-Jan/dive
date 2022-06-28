using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using Xunit;

namespace Dive.Tests.Integration.Posts
{
    public class PostDetailTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Client { get; }

        public DiveContext Context { get; }

        public PostDetailTest(TestApplicationFixture app)
        {
            Client = app.CreateClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task related_posts_are_recommended()
        {
            var tag = new Tag { Name = "example" };
            Context.Tags.Add(tag);
            var post = new Post { Title = "Foo", Body = "Foo", UserId = 1, Tags = new List<Tag> { tag } };
            var recommendation = new Post { Title = "Bar", Body = "Bar", UserId = 2, Tags = new List<Tag> { tag } };
            Context.Posts.Add(post);
            Context.Posts.Add(recommendation);
            Context.SaveChanges();

            var response = await Client.GetAsync($"/questions/{post.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();
            TestUtilities.AssertHtmlContains("Bar", html);
        }
    }
}
