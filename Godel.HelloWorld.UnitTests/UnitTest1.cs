using NUnit.Framework;

namespace Godel.HelloWorld.UnitTests
{
    [TestFixture(Category = "Unit")]
    public class UnitTest1
    {
        [Test]
        public void Test1()
        {
            Assert.AreEqual(1 + 1, 2);
        }
    }
}
