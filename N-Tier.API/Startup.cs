using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N_Tier.API.Filters;
using N_Tier.API.Middleware;
using N_Tier.Application.Models.Validators.TodoList;
using N_Tier.Common;

namespace N_Tier.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(
                    config => config.Filters.Add(typeof(ValidateModelAttribute))
                )
                .AddFluentValidation(
                    options => options.RegisterValidatorsFromAssemblyContaining<CreateTodoListModelValidator>()
                );

            services.AddSwagger();

            services.AddDatabase(_configuration);

            services.AddRepositories();

            services.AddServices();

            services.AddIdentity();

            services.AddJwt(_configuration);

            services.RegisterAutoMapper();

            services.AddEmailConfiguration(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "N-Tier V1");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<PerformanceMiddleware>();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
