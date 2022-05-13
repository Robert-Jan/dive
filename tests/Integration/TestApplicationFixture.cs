using Dive.App.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;

namespace Dive.Tests.Integration
{
    public class TestApplicationFixture : WebApplicationFactory<App.Startup>
    {
        private ServiceProvider _provider;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                _provider = services.BuildServiceProvider();

                services.AddMvc(options =>
                {
                    options.Filters.Remove(new AutoValidateAntiforgeryTokenAttribute());
                });

                using var scope = _provider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<DiveContext>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                TestContextUtilities.InitializeDatabase(context);
            });

            base.ConfigureWebHost(builder);
        }

        public DiveContext CreateContext()
        {
            return _provider.GetRequiredService<DiveContext>();
        }

        public HttpClient CreateAuthenticatedClient()
        {
            return WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestAuthentication";
                        options.DefaultChallengeScheme = "TestAuthentication";
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestAuthentication", null);
                });
            }).CreateClient();
        }
    }
}