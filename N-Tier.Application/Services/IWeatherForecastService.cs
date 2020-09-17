using N_Tier.Application.Models.WeatherForecast;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N_Tier.Application.Services
{
    public interface IWeatherForecastService
    {
        public Task<IEnumerable<WeatherForecastResponseModel>> GetAsync();
    }
}
