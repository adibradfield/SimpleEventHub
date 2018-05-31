using System.Collections.Generic;

namespace SimpleEventHub
{
    public interface IEventHub
    {
        void AddEvent<T>(T eventInfo);
        void Register(IHandle handler);
        void Register<T>(IHandle<T> handler);
        void Register<T>(IHandleAsync<T> handler);
        IEnumerable<IHandle> GetRegisteredHandlers<T>();
    }
}