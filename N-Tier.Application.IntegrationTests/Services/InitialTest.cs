using FluentAssertions;
using Xunit;

namespace N_Tier.Application.IntegrationTests
{
    public class InitialTest
    {
        [Fact]
        public void Test1()
        {
            true.Should().BeTrue();
        }
    }
}
