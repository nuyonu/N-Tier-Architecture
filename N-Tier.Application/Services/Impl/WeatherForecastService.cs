using N_Tier.Application.Models.WeatherForecast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N_Tier.Application.Services.Impl
{

    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] _summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastService(ITemplateService htmlTemplateService)
        {
            htmlTemplateService.GetTemplateAsync("test");
        }

        public async Task<IEnumerable<WeatherForecastResponseModel>> GetAsync()
        {
            var rng = new Random();

            await Task.Delay(500);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecastResponseModel
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = _summaries[rng.Next(_summaries.Length)]
            });
        }
    }
}
