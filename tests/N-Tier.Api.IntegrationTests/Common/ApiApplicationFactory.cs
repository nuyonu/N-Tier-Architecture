using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N_Tier.Api.IntegrationTests.Helpers.Services;
using N_Tier.Application.Services;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.Api.IntegrationTests.Common;

public class ApiApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly string _inMemoryDatabaseName = Guid.NewGuid().ToString().Replace("-", string.Empty);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseTestServer();
        
        builder.ConfigureAppConfiguration((context, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(
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

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));

            services.Remove(dbContextDescriptor);

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseInMemoryDatabase(_inMemoryDatabaseName);
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<IEmailService, EmailTestService>();
            services.AddScoped<ITemplateService, TemplateTestService>();
        });
    }
    
    
}
