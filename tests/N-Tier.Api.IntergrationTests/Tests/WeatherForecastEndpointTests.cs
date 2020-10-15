using FluentAssertions;
using N_Tier.Api.IntergrationTests.Config;
using N_Tier.Api.IntergrationTests.Helpers;
using N_Tier.Application.Models;
using N_Tier.Application.Models.WeatherForecast;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Api.IntergrationTests.Tests
{
    [TestFixture]
    public class WeatherForecastEndpointTests : BaseOneTimeSetup
    {
        [Test]
        public async Task Login_Should_Return_User_Informations_And_Token()
        {
            // Arrange

            // Act
            var apiResponse = await _client.GetAsync("/api/WeatherForecast");

            // Assert
            var response = JsonConvert.DeserializeObject<ApiResult<IEnumerable<WeatherForecastResponseModel>>>(await apiResponse.Content.ReadAsStringAsync());
            CheckResponse.Succeded(response);
            response.Result.Should().HaveCount(5);
        }
    }
}
