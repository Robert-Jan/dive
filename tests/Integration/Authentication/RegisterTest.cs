using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dive.App.Data;
using Xunit;

namespace Dive.Tests.Integration.Authentication
{
    public class RegisterTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Client { get; }

        public DiveContext Context { get; }

        public RegisterTest(TestApplicationFixture app)
        {
            Client = app.CreateClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task a_visitor_can_create_an_account()
        {
            var response = await Client.PostAsync("/register", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", "jake@example.com"),
                new KeyValuePair<string, string>("username", "jake"),
                new KeyValuePair<string, string>("password", "Test1234!"),
            }));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("/login", TestUtilities.ResponseUrl(response));
            Assert.Equal("jake", Context.Users.Where(u => u.Id == 3).First().UserName);
        }
    }
}
