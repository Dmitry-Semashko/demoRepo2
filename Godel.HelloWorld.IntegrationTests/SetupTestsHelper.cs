using System.Linq;
using AutoFixture;

namespace Godel.HelloWorld.IntegrationTests
{
    public static class SetupTestsHelper
    {
        public static Fixture CreateFixture()
        {
            var fixture = new Fixture();
            fixture
                .Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            SetupAutoFixtureIdGenerationStrategy(fixture);

            return fixture;
        }

        public static void SetupAutoFixtureIdGenerationStrategy(Fixture fixture)
        {
            //there is an exception when add new entities to db that Id is already tracked
            //this is because of poor random function of fixture
            //let's generate Ids by ourselves by registering methods for it
            //but we need to generate our ids from the 'end' because
            //db context starts its id generation from 0 and goes on
            //but db context does not know anything our generated ids
            //and in order our ids not to intersect db context ids
            //we need to generate ours from them max values (not from 0 and above)
            var currentId = long.MaxValue;
            fixture.Register(() => currentId--);
            fixture.Register(() => (ulong)currentId--);
        }
    }
}