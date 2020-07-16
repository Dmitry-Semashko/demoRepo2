using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using AutoFixture;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Godel.HelloWorld.IntegrationTests
{
    public class TestFixture : IDisposable
    {
        private const string SOLUTION_NAME = "Godel.HelloWorld.sln";

        private IServiceScope scope;

        public readonly TestServer Server;

        public readonly HttpClient Client;

        public readonly Fixture Fixture;

        public JsonSerializerSettings JsonSerializerSettings
        {
            get
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    Formatting = Formatting.Indented
                };

                settings.Converters.Add(new StringEnumConverter
                {
                    AllowIntegerValues = false
                });

                return settings;
            }
        }

        public TestFixture()
        {
            Assembly startupAssembly = typeof(Startup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(startupAssembly);

            Server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestStartup>()
                .UseContentRoot(contentRoot));

            Client = Server.CreateClient();

            Fixture = SetupTestsHelper.CreateFixture();
        }

        public T GetService<T>()
        {
            if (Server?.Host == null)
            {
                throw new Exception("The Server has not been started. Please call CreateClient() on this factory first");
            }
            return Server.Host.Services.GetService<T>();
        }

        public T GetScopedService<T>()
        {
            if (Server?.Host == null)
            {
                throw new Exception("The Server has not been started. Please call CreateClient() on this factory first");
            }

            if (scope == null)
            {
                scope = Server.Host.Services.CreateScope();
            }

            return scope.ServiceProvider.GetService<T>();
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }

        private static string GetProjectPath(Assembly startupAssembly)
        {
            //Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            //Get currently executing test project path
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            //Find the folder which contains the solution file. We then use this information to find the 
            //target project which we want to test
            DirectoryInfo directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, SOLUTION_NAME));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, projectName));
                }
                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo?.Parent != null);

            throw new Exception($"Solution root could not be located using application root {applicationBasePath}");
        }
    }
}
