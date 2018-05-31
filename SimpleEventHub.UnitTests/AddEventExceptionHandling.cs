using System;
using NUnit.Framework;
using SimpleEventHub.UnitTests.ModelEvents;
using SimpleEventHub.UnitTests.TestHandlers;

namespace SimpleEventHub.UnitTests
{
    [TestFixture]
    public class AddEventExceptionHandling
    {
        [Test]
        [TestCase(0, ExpectedResult = null)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 2)]
        public int? ExceptionIsAggregated(int instances)
        {
            var hub = new EventHub();

            for (var i = 0; i < instances; i++)
            {
                var handler = new SyncThrowsExceptionHandler();
                hub.Register(handler);
            }

            try
            {
                hub.AddEvent(new BaseEvent());
            }
            catch (AggregateException e)
            {
                return e.InnerExceptions.Count;
            }

            return null;
        }
    }
}
