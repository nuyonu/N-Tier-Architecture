using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N_Tier.Application.MappingProfiles;
using N_Tier.Application.Services;
using N_Tier.Application.Services.Impl;
using N_Tier.Common.ConfigurationModels;
using N_Tier.Infrastructure.Identity;
using N_Tier.Infrastructure.Persistence;
using N_Tier.Infrastructure.Repositories;
using N_Tier.Infrastructure.Repositories.Impl;
using System;

namespace N_Tier.Common
{
    public static class DependencyInjectionConfigurations
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            services.AddScoped<ITodoListService, TodoListService>();
            services.AddScoped<ITodoItemService, TodoItemService>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITodoItemRepository, TodoItemRepository>();
            services.AddScoped<ITodoListRepository, TodoListRepository>();
        }

        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseConfig = configuration.GetSection("Database").Get<DatabaseConfiguration>();

            if (databaseConfig.UseInMemoryDatabase)
            {
                services.AddDbContext<DatabaseContext>(options =>
                    options.UseInMemoryDatabase("NTierDatabase"));
            }
            else
            {
                services.AddDbContext<DatabaseContext>(options =>
                    options.UseSqlServer(databaseConfig.ConnectionString,
                    opt => opt.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)));
            }
        }

        public static void RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(TodoListProfile).Assembly);
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            // TODO update RequireConfirmedAccount = true
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<DatabaseContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // TODO update password settings
                options.Password.RequireDigit = false; // true
                options.Password.RequireLowercase = false; // true
                options.Password.RequireNonAlphanumeric = false; // true
                options.Password.RequireUppercase = false; // true
                options.Password.RequiredLength = 2; // 6
                options.Password.RequiredUniqueChars = 0; //1

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false; // true
            });
        }
    }
}
