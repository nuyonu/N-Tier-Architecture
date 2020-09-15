using Microsoft.Extensions.DependencyInjection;
using N_Tier.Application.Services;
using N_Tier.Application.Services.Impl;

namespace N_Tier.Common
{
    public static class DependencyInjectionConfigurations
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IWeatherForecastService, WeatherForecastService>();
        }
    }
}
