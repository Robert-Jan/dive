using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dive.Tests.Integration.Authentication
{
    public class LogoutTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Client { get; }

        public LogoutTest(TestApplicationFixture app)
        {
            Client = app.CreateAuthenticatedClient();
        }

        [Fact]
        public async Task an_authenticated_user_can_logout_of_their_account()
        {
            var response = await Client.GetAsync("/logout");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("/", TestUtilities.ResponseUrl(response));
        }
    }
}
