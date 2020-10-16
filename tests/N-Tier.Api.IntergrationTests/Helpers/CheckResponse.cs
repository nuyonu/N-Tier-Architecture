using FluentAssertions;
using N_Tier.Application.Models;

namespace N_Tier.Api.IntergrationTests.Helpers
{
    public class CheckResponse
    {
        public static void Succeded<T>(ApiResult<T> result, int code = 200)
        {
            result.Code.Should().Be(code);
            result.Succeeded.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.Result.Should().NotBe(default);
        }

        public static void Failure<T>(ApiResult<T> result, int code)
        {
            result.Code.Should().Be(code);
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Result.Should().Be(default);
        }
    }
}
