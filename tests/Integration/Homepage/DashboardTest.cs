using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dive.Tests.Integration.Authentication
{
    public class DashboardTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Client { get; }

        public DashboardTest(TestApplicationFixture app)
        {
            Client = app.CreateClient();
        }

        [Fact]
        public async Task an_greeting_is_given()
        {
            var response = await Client.GetAsync("/");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("/", TestUtilities.ResponseUrl(response));
            TestUtilities.AssertHtmlContains("Hello .NET", await response.Content.ReadAsStringAsync());
        }
    }
}
