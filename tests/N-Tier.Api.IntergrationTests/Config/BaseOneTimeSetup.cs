using FizzWare.NBuilder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N_Tier.Api.IntergrationTests.Helpers.Services;
using N_Tier.API;
using N_Tier.Application.Helpers;
using N_Tier.Application.Services;
using N_Tier.DataAccess.Identity;
using N_Tier.DataAccess.Persistence;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace N_Tier.Api.IntergrationTests.Config
{
    [SetUpFixture]
    public class BaseOneTimeSetup
    {
        public IHost _host;
        public HttpClient _client;

        [OneTimeSetUp]
        public async Task RunBeforeAnyTestsAsync()
        {
            _host = await GetNewHostAsync();

            _client = await GetNewClient(_host);
        }

        protected async Task<IHost> GetNewHostAsync()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<Startup>();
                    webHost.ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        configBuilder.AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                ["Database:UseInMemoryDatabase"] = "true",
                                ["JwtConfiguration:SecretKey"] = "Super secret token key",
                                ["SmtpSettings:Server"] = "",
                                ["SmtpSettings:Port"] = "548",
                                ["SmtpSettings:SenderName"] = "",
                                ["SmtpSettings:SenderEmail"] = "",
                                ["SmtpSettings:Username"] = "",
                                ["SmtpSettings:Password"] = "",
                            });
                    }); ;

                    webHost.ConfigureTestServices(services =>
                    {
                        services.AddScoped<IEmailService, EmailTestService>();
                        services.AddScoped<ITemplateService, TemplateTestService>();
                    });

                });

            var host = await hostBuilder.StartAsync();

            var context = host.Services.GetRequiredService<DatabaseContext>();

            var userManager = host.Services.GetRequiredService<UserManager<ApplicationUser>>();

            var databaseUser = Builder<ApplicationUser>.CreateNew()
                .With(u => u.EmailConfirmed = true)
                .With(u => u.Email = "nuyonu@gmail.com")
                .With(u => u.UserName = "nuyonu")
                .Build();

            await userManager.CreateAsync(databaseUser, "Password.1!");

            await context.SaveChangesAsync();

            return host;
        }

        public async Task<HttpClient> GetNewClient(IHost host)
        {
            var configuration = host.Services.GetRequiredService<IConfiguration>();

            var userManager = host.Services.GetRequiredService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync("nuyonu");

            var client = host.GetTestClient();

            var token = JwtHelper.GenerateToken(user, configuration);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        { }
    }
}
