using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using Xunit;

namespace Dive.Tests.Integration.Authentication
{
    public class DashboardTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Client { get; }

        public DiveContext Context { get; }

        public DashboardTest(TestApplicationFixture app)
        {
            Client = app.CreateClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task posts_are_displayed_on_the_homepage()
        {
            Context.Posts.Add(new Post { Title = "Foo", Body = "Lorem ipsum", UserId = 1 });
            Context.Posts.Add(new Post { Title = "Bar", Body = "Lorem ipsum", UserId = 1 });
            Context.SaveChanges();

            var response = await Client.GetAsync("/");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();
            TestUtilities.AssertHtmlContains("Foo", html);
            TestUtilities.AssertHtmlContains("Bar", html);
        }
    }
}
