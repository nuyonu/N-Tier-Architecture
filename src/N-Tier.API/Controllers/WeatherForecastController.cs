using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models;
using N_Tier.Application.Models.WeatherForecast;
using N_Tier.Application.Services;

namespace N_Tier.API.Controllers;

[Authorize]
public class WeatherForecastController(IWeatherForecastService weatherForecastService) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(
            ApiResult<IEnumerable<WeatherForecastResponseModel>>.Success(await weatherForecastService.GetAsync()));
    }
}
