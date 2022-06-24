using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using Xunit;

namespace Dive.Tests.Integration.Search
{
    public class SearchTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Client { get; }

        public DiveContext Context { get; }

        public SearchTest(TestApplicationFixture app)
        {
            Client = app.CreateClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task posts_can_be_searched_on_title_and_body()
        {
            Context.Posts.Add(new Post { Title = "Lorem ipsum 1", Body = "Foo", UserId = 1 });
            Context.Posts.Add(new Post { Title = "Bar", Body = "Lorem ipsum 2", UserId = 2 });
            Context.Posts.Add(new Post { Title = "Baz", Body = "Baz", UserId = 1 });
            Context.SaveChanges();

            var response = await Client.GetAsync("/search?q=lorem");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();
            TestUtilities.AssertHtmlContains("Lorem ipsum 1", html);
            TestUtilities.AssertHtmlContains("Lorem ipsum 2", html);
            TestUtilities.AssertHtmlDoesNotContains("Baz", html);
        }

        [Fact]
        public async Task posts_can_be_searched_on_the_connected_tags()
        {
            var tag = new Tag { Name = "example" };
            Context.Tags.Add(tag);
            Context.Posts.Add(new Post { Title = "Lorem ipsum", Body = "Foo bar", UserId = 1, Tags = new List<Tag> { tag } });
            Context.SaveChanges();

            var response = await Client.GetAsync("/search?q=example");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();
            TestUtilities.AssertHtmlContains("Lorem ipsum", html);
        }
    }
}
