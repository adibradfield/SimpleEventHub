using SimpleEventHub.UnitTests.ModelEvents;

namespace SimpleEventHub.UnitTests.TestHandlers
{
    class SyncBothCountingHandler : IHandle<BaseEvent>, IHandle<DerivedEvent>
    {
        public int BaseHandleCount { get; set; }
        public int DerivedHandleCount { get; set; }
        public int TotalHandleCount => BaseHandleCount + DerivedHandleCount;

        public void Handle(BaseEvent eventInfo)
        {
            BaseHandleCount++;
        }

        public void Handle(DerivedEvent eventInfo)
        {
            DerivedHandleCount++;
        }
    }
}
