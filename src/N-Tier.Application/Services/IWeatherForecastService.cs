using N_Tier.Application.Models.WeatherForecast;

namespace N_Tier.Application.Services;

public interface IWeatherForecastService
{
    public Task<IEnumerable<WeatherForecastResponseModel>> GetAsync();
}
