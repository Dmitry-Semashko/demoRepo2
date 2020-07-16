using System.Threading.Tasks;
using Godel.HelloWorld.IntegrationTests.Extensions;
using Godel.HelloWorld.Models.TestController;
using NUnit.Framework;

namespace Godel.HelloWorld.IntegrationTests
{
    [TestFixture(Category = "Integration")]
    public class TestControllerTests
    {
        private TestFixture testFixture;
        public Response result;

        [OneTimeSetUp]
        public async Task SetupTests()
        {
            testFixture = new TestFixture();

            
        }

        [Test]
        public async Task ValidateResponse()
        {
            var httpResponse = await testFixture.Client.GetAsync("/api/test");
            result = await httpResponse.GetResultData<Response>();

            Assert.AreEqual(result.Action, "Test");
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            testFixture.Dispose();
        }
    }
}
