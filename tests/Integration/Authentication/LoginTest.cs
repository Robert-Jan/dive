using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dive.Tests.Integration.Authentication
{
    public class LoginTest : IClassFixture<TestFixture>
    {
        public HttpClient Client { get; }

        public LoginTest(TestFixture factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task AnExistingUserCanLoginWithTheirCredentials()
        {
            var response = await Client.GetAsync("/login");

            response.EnsureSuccessStatusCode();
        }
    }
}
