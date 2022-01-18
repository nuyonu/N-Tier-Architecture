using FluentAssertions;
using N_Tier.Application.Models;

namespace N_Tier.Api.IntegrationTests.Helpers;

public static class CheckResponse
{
    public static void Succeeded<T>(ApiResult<T> result)
    {
        result.Succeeded.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Result.Should().NotBe(default);
    }

    public static void Failure<T>(ApiResult<T> result)
    {
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThan(0);
        result.Result.Should().Be(default);
    }
}
