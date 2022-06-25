using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using Xunit;

namespace Dive.Tests.Integration.Search
{
    public class TagsOverviewTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Client { get; }

        public DiveContext Context { get; }

        public TagsOverviewTest(TestApplicationFixture app)
        {
            Client = app.CreateClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task tags_are_viewed_on_the_tag_overview_page()
        {
            Context.Tags.Add(new Tag { Name = "foo" });
            Context.Tags.Add(new Tag { Name = "bar" });
            Context.SaveChanges();

            var response = await Client.GetAsync("/tags");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();
            TestUtilities.AssertHtmlContains("foo", html);
            TestUtilities.AssertHtmlContains("bar", html);
        }
    }
}
