using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using N_Tier.Application.Common.Email;
using N_Tier.Application.MappingProfiles;
using N_Tier.Application.Services;
using N_Tier.Application.Services.Impl;
using N_Tier.Common.ConfigurationModels;
using N_Tier.Infrastructure.Identity;
using N_Tier.Infrastructure.Persistence;
using N_Tier.Infrastructure.Repositories;
using N_Tier.Infrastructure.Repositories.Impl;
using N_Tier.Shared.Services;
using N_Tier.Shared.Services.Impl;
using System;
using System.Text;

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
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITemplateService, TemplateService>();
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

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer YOUR_TOKEN')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
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

        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration.GetValue<string>("JwtConfiguration:SecretKey");

            var key = Encoding.ASCII.GetBytes(secretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection("SmtpSettings").Get<SmtpSettings>());
        }
    }
}
