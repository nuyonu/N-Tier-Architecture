using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N_Tier.API;
using N_Tier.Api.IntegrationTests.Config.Constants;
using N_Tier.Api.IntegrationTests.Helpers.Services;
using N_Tier.Application.Helpers;
using N_Tier.Application.Services;
using N_Tier.DataAccess.Identity;
using N_Tier.DataAccess.Persistence;
using NUnit.Framework;

namespace N_Tier.Api.IntegrationTests.Config;

[SetUpFixture]
public class BaseOneTimeSetup
{
    protected HttpClient Client;
    protected IHost Host;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTestsAsync()
    {
        Host = await GetNewHostAsync();

        Client = await GetNewClient(Host);
    }

    protected static async Task<IHost> GetNewHostAsync()
    {
        var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.UseStartup<Startup>();
                webHost.ConfigureAppConfiguration((_, configBuilder) =>
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
                            ["SmtpSettings:Password"] = ""
                        });
                });
                ;

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
            .With(u => u.Email = UserConstants.DefaultUserDb.Email)
            .With(u => u.UserName = UserConstants.DefaultUserDb.Username)
            .Build();

        await userManager.CreateAsync(databaseUser, UserConstants.DefaultUserDb.Password);

        await context.SaveChangesAsync();

        return host;
    }

    private static async Task<HttpClient> GetNewClient(IHost host)
    {
        var configuration = host.Services.GetRequiredService<IConfiguration>();

        var userManager = host.Services.GetRequiredService<UserManager<ApplicationUser>>();

        var user = await userManager.FindByNameAsync(UserConstants.DefaultUserDb.Username);

        var client = host.GetTestClient();

        var token = JwtHelper.GenerateToken(user, configuration);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests() { }
}
