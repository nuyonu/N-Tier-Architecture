using FizzWare.NBuilder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N_Tier.Api.IntegrationTests.Helpers;
using N_Tier.Api.IntegrationTests.Helpers.Services;
using N_Tier.API;
using N_Tier.Application.Models;
using N_Tier.Application.Models.User;
using N_Tier.Application.Services;
using N_Tier.DataAccess.Identity;
using N_Tier.DataAccess.Persistence;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace N_Tier.Api.IntegrationTests.Config
{
    public class SingletonConfig
    {
        private static IHost _host;

        private static HttpClient _client;

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
            }

            return _host;
        }

        public static async Task<HttpClient> GetAuthenticatedClientInstanceAsync()
        {
            if (_client == null)
            {
                var host = await GetHostInstanceAsync();

                var client = host.GetTestClient();

                var loginUserModel = Builder<LoginUserModel>.CreateNew()
                                                            .With(cu => cu.Username = "nuyonu")
                                                            .With(cu => cu.Password = "Password.1!")
                                                            .Build();

                var apiAuthenticateResponse = await client.PostAsync("/api/users/authenticate", new JsonContent(loginUserModel));
                var responseAuthenticate = JsonConvert.DeserializeObject<ApiResult<LoginResponseModel>>(await apiAuthenticateResponse.Content.ReadAsStringAsync());

                var token = responseAuthenticate.Result.Token;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                _client = client;
            }

            return _client;
        }
    }
}
