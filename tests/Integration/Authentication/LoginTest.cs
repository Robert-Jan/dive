using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dive.Tests.Integration.Authentication
{
    public class LoginTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Client { get; }

        public LoginTest(TestApplicationFixture app)
        {
            Client = app.CreateClient();
        }

        [Fact]
        public async Task an_existing_user_can_login_with_their_credentials()
        {
            var response = await Client.PostAsync("/login", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "john"),
                new KeyValuePair<string, string>("password", "Test1234!"),
            }));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("/", TestUtilities.ResponseUrl(response));
        }

        [Fact]
        public async Task an_existing_user_with_the_wrong_password_is_denied()
        {
            var response = await Client.PostAsync("/login", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "john"),
                new KeyValuePair<string, string>("password", "WrongPassword1234!"),
            }));

            Assert.Equal("/login", TestUtilities.ResponseUrl(response));
            TestUtilities.AssertHtmlContains("The username or password is incorrect", await response.Content.ReadAsStringAsync());
        }
    }
}
