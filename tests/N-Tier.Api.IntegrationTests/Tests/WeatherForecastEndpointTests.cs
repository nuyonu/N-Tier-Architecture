using FluentAssertions;
using N_Tier.Api.IntegrationTests.Config;
using N_Tier.Api.IntegrationTests.Helpers;
using N_Tier.Application.Models;
using N_Tier.Application.Models.WeatherForecast;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace N_Tier.Api.IntegrationTests.Tests
{
    public class WeatherForecastEndpointTests
    {
        [Fact]
        public async Task Login_Should_Return_User_Informations_And_Token()
        {
            // Arrange
            var client = await SingletonConfig.GetAuthenticatedClientInstanceAsync();

            // Act
            var apiResponse = await client.GetAsync("/api/WeatherForecast");

            // Assert
            var response = JsonConvert.DeserializeObject<ApiResult<IEnumerable<WeatherForecastResponseModel>>>(await apiResponse.Content.ReadAsStringAsync());
            CheckResponse.Succeded(response);
            response.Result.Should().HaveCount(5);
        }
    }
}
