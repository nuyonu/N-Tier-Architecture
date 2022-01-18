using FluentValidation.AspNetCore;
using N_Tier.API.Filters;
using N_Tier.API.Middleware;
using N_Tier.Application;
using N_Tier.Application.Models.Validators;
using N_Tier.DataAccess;

namespace N_Tier.API;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(
                config => config.Filters.Add(typeof(ValidateModelAttribute))
            )
            .AddFluentValidation(
                options => options.RegisterValidatorsFromAssemblyContaining<IValidationsMarker>()
            );

        services.AddSwagger();

        services.AddDataAccess(_configuration)
            .AddApplication(_env);

        services.AddJwt(_configuration);

        services.AddEmailConfiguration(_configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseCors(corsPolicyBuilder =>
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
        );

        app.UseSwagger();

        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "N-Tier V1"); });

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseMiddleware<PerformanceMiddleware>();

        app.UseMiddleware<TransactionMiddleware>();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
