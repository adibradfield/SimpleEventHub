using System;
using SimpleEventHub.UnitTests.ModelEvents;

namespace SimpleEventHub.UnitTests.TestHandlers
{
    class SyncThrowsExceptionHandler : IHandle<BaseEvent>
    {
        public void Handle(BaseEvent eventInfo)
        {
            throw new NotImplementedException();
        }
    }
}
