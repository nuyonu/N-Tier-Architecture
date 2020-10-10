using Microsoft.Extensions.Hosting;

namespace N_Tier.Api.IntegrationTests.Config
{
    public class BaseTest
    {
        private readonly IHost _host;
        private readonly object _context;

        public BaseTest()
        {
            //_host = await SingletonConfig.GetHostInstanceAsync();

            //_context = _host.Services.GetRequiredService<DatabaseContext>();
        }
    }
}
