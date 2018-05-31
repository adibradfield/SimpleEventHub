using SimpleEventHub.UnitTests.ModelEvents;

namespace SimpleEventHub.UnitTests.TestHandlers
{
    class SyncBaseCountingHandler : IHandle<BaseEvent>
    {
        public int HandleCount { get; set; }
        public void Handle(BaseEvent eventInfo)
        {
            HandleCount++;
        }
    }
}
