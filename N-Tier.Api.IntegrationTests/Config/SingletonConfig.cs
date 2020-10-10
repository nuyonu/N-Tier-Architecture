using FizzWare.NBuilder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N_Tier.Api.IntegrationTests.Helpers.Services;
using N_Tier.API;
using N_Tier.Application.Services;
using N_Tier.DataAccess.Identity;
using N_Tier.DataAccess.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N_Tier.Api.IntegrationTests.Config
{
    public class SingletonConfig
    {
        private static IHost _host;
        private SingletonConfig() { }

        public static async Task<IHost> GetHostInstanceAsync()
        {
            if (_host == null)
            {
                var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    // Add TestServer
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

                    // configure the services after the startup has been called.
                    webHost.ConfigureTestServices(services =>
                    {
                        // register the test one specifically
                        services.AddScoped<IEmailService, EmailTestService>();
                        services.AddScoped<ITemplateService, TemplateTestService>();
                    });

                });

                // Build and start the IHost
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

                _host = host;

                return host;
            }

            return _host;
        }
    }
}
