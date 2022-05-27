using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dive.App.Data;
using Xunit;

namespace Dive.Tests.Integration.Settings
{
    public class ProfileSettingsTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Client { get; }

        public DiveContext Context { get; }

        public ProfileSettingsTest(TestApplicationFixture app)
        {
            Client = app.CreateAuthenticatedClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task a_user_can_change_their_profile_settings()
        {
            var user = Context.Users.Where(u => u.Id == 1).First();
            Assert.Equal("john@example.com", user.Email);
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);

            var response = await Client.PostAsync("/settings", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", "foo@example.com"),
                new KeyValuePair<string, string>("first_name", "Foo"),
                new KeyValuePair<string, string>("last_name", "Bar"),
                new KeyValuePair<string, string>("current_password", "Test1234!"),
                new KeyValuePair<string, string>("url", "/"),
            }));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Context.Entry(user).Reload();
            Assert.Equal("foo@example.com", user.Email);
            Assert.Equal("Foo", user.FirstName);
            Assert.Equal("Bar", user.LastName);
        }
    }
}
