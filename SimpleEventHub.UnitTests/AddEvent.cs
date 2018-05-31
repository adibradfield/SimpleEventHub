using NUnit.Framework;
using SimpleEventHub.UnitTests.ModelEvents;
using SimpleEventHub.UnitTests.TestHandlers;

namespace SimpleEventHub.UnitTests
{
    [TestFixture()]
    public class AddEvent
    {
        [Test]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 2)]
        public int ExactClassMatchHandled(int iterations)
        {
            var hub = new EventHub();
            var handler = new SyncDerivedCountingHandler();
            hub.Register(handler);

            for (var i = 0; i < iterations; i++)
            {
                hub.AddEvent(new DerivedEvent());
                hub.AddEvent(new BaseEvent()); //Should not handle this
            }

            return handler.HandleCount;
        }

        [Test]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 2)]
        public int DerivedClassMatchHandled(int iterations)
        {
            var hub = new EventHub();
            var handler = new SyncBaseCountingHandler();
            hub.Register(handler);

            for (var i = 0; i < iterations; i++)
            {
                hub.AddEvent(new DerivedEvent());
                hub.AddEvent(42); //Should not handle this
            }

            return handler.HandleCount;
        }

        [Test]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 3)]
        [TestCase(2, ExpectedResult = 6)]
        public int CatchAllHandled(int iterations)
        {
            var hub = new EventHub();
            var handler = new SyncAllCountingHandler();
            hub.Register(handler);

            for (var i = 0; i < iterations; i++)
            {
                hub.AddEvent(new BaseEvent());
                hub.AddEvent(new DerivedEvent());
                hub.AddEvent(42);
            }

            return handler.HandleCount;
        }
    }
}
