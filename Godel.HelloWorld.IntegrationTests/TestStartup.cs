using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Godel.HelloWorld.IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
