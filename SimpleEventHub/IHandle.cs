using System.Threading.Tasks;

namespace SimpleEventHub
{
    public interface IHandle<in T> : IHandle
    {
        void Handle(T eventInfo);
    }

    public interface IHandle { }

    public interface IHandleAsync<in T> : IHandle
    {
        Task Handle(T eventInfo);
    }
}
