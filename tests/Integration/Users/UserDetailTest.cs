using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using Xunit;

namespace Dive.Tests.Integration.Users
{
    public class UserDetailTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Client { get; }

        public DiveContext Context { get; }

        public UserDetailTest(TestApplicationFixture app)
        {
            Client = app.CreateClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task the_posts_of_the_user_are_displayed()
        {
            Context.Posts.Add(new Post { Title = "Foo", Body = "Lorem ipsum", UserId = 1 });
            Context.Posts.Add(new Post { Title = "Bar", Body = "Lorem ipsum", UserId = 1 });
            Context.Posts.Add(new Post { Title = "Baz", Body = "Lorem ipsum", UserId = 2 });
            Context.SaveChanges();

            var response = await Client.GetAsync("/users/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();
            TestUtilities.AssertHtmlContains("Foo", html);
            TestUtilities.AssertHtmlContains("Bar", html);
            TestUtilities.AssertHtmlDoesNotContains("Baz", html);
        }
    }
}
