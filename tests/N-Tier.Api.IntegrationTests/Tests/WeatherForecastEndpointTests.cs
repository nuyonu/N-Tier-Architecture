using FluentAssertions;
using N_Tier.Application.Models;
using N_Tier.Application.Models.WeatherForecast;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using N_Tier.Api.IntegrationTests.Config;
using N_Tier.Api.IntegrationTests.Helpers;

namespace N_Tier.Api.IntegrationTests.Tests
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
