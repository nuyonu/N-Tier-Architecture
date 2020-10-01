using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models;
using N_Tier.Application.Models.WeatherForecast;
using N_Tier.Application.Services;

namespace N_Tier.API.Controllers
{
    [Authorize]
    public class WeatherForecastController : ApiController
    {
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(IWeatherForecastService weatherForecastService)
        {
            _weatherForecastService = weatherForecastService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
            => Ok(ApiResult<IEnumerable<WeatherForecastResponseModel>>.Success200(await _weatherForecastService.GetAsync()));
    }
}
