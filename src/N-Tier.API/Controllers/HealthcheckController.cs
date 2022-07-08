using Microsoft.AspNetCore.Mvc;

namespace N_Tier.API.Controllers;

public class HealthcheckController : ApiController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
