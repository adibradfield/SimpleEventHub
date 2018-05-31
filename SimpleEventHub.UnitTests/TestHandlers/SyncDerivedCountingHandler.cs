using SimpleEventHub.UnitTests.ModelEvents;

namespace SimpleEventHub.UnitTests.TestHandlers
{
    class SyncDerivedCountingHandler : IHandle<DerivedEvent>
    {
        public int HandleCount { get; set; }
        public void Handle(DerivedEvent eventInfo)
        {
            HandleCount++;
        }
    }
}
