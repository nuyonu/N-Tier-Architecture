using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace N_Tier.Api.IntergrationTests.Config
{
    public static class TestConfig
    {
        public static IHost Host { get; set; }

        public static HttpClient Client { get; set; }
    }
}
