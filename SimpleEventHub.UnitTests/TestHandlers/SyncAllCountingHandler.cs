namespace SimpleEventHub.UnitTests.TestHandlers
{
    class SyncAllCountingHandler : IHandle<object>
    {
        public int HandleCount { get; set; }
        public void Handle(object eventInfo)
        {
            HandleCount++;
        }
    }
}
