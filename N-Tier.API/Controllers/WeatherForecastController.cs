using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N_Tier.Application.Models.WeatherForecast;
using N_Tier.Application.Services;

namespace N_Tier.API.Controllers
{
    [Authorize]
    public class WeatherForecastController : ApiController
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecastResponseModel>>> Get() => Ok(await _weatherForecastService.GetAsync());
    }
}
