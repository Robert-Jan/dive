using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Dive.App.Data;
using Dive.App.Models;
using Dive.App.ActionFilters;
using Dive.App.Repositories;

namespace Dive.App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("Properties/appSettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("/dive/storage/DataProtectionKeys"))
                .SetApplicationName("Dive")
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });

            services.AddDbContext<DiveContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL"));
                options.UseSnakeCaseNamingConvention();
            });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DiveContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.Cookie.Name = "session";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
                options.FormFieldName = "csrf";
                options.Cookie.Name = "csrf";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add<ViewBagActionFilter>();
            });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddTransient<IPostRepository, EFPostRepository>();
            services.AddTransient<ITagRepository, EFTagRepository>();
            services.AddTransient<IUserRepository, EFUserRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DiveContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                context.Database.Migrate();
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider("/dive/resources/wwwroot"),
                RequestPath = new PathString("")
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
