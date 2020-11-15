using N_Tier.Application.Helpers;
using N_Tier.Application.Models.WeatherForecast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N_Tier.Application.Services.Impl
{

    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly List<string> _summaries;

        public WeatherForecastService()
        {
            _summaries = new List<string> { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        }

        public async Task<IEnumerable<WeatherForecastResponseModel>> GetAsync()
        {
            await Task.Delay(500);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecastResponseModel
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = RandomGenerator.GenerateInteger(-20, 55),
                Summary = _summaries[RandomGenerator.GenerateInteger(0, _summaries.Count)]
            });
        }
    }
}
