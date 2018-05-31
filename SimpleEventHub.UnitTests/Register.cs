using System.Linq;
using NUnit.Framework;
using SimpleEventHub.UnitTests.ModelEvents;
using SimpleEventHub.UnitTests.TestHandlers;

namespace SimpleEventHub.UnitTests
{
    [TestFixture]
    public class Register
    {
        [Test]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 2)]
        public int RegisterSingle(int instances)
        {
            var hub = new EventHub();

            for (var i = 0; i < instances; i++)
            {
                var handler = new SyncBaseCountingHandler();
                hub.Register(handler);
            }

            return hub.GetRegisteredHandlers<BaseEvent>().Count();
        }
    }
}
