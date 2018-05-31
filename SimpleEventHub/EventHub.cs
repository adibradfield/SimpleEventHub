using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEventHub
{
    public class EventHub : IEventHub
    {
        private readonly Dictionary<Type, List<IHandle>> _registrationList = new Dictionary<Type, List<IHandle>>();

        public void Register<T>(IHandle<T> handler)
        {
            AddHandler(typeof(T), handler);
        }
        public void Register<T>(IHandleAsync<T> handler)
        {
            AddHandler(typeof(T), handler);
        }

        public void Register(IHandle handler)
        {
            var type = handler.GetType();
            var handlerTypes = type.GetInterfaces()
                .Where(i => i.IsGenericType)
                .Where(i => i.GetGenericTypeDefinition() == typeof(IHandle<>) || i.GetGenericTypeDefinition() == typeof(IHandleAsync<>))
                .Select(i => i.GetGenericArguments().First());
            foreach (var handlerType in handlerTypes)
            {
                AddHandler(handlerType, handler);
            }
        }

        public void AddEvent<T>(T eventInfo)
        {
            var exceptions = new List<Exception>();
            var asyncTasks = new List<Task>();

            foreach (var entry in GetHandlersAssignableToType(typeof(T)))
            {
                var asyncType = typeof(IHandleAsync<>).MakeGenericType(entry.Key);
                var asyncProxy = asyncType.GetMethod("Handle");
                Debug.Assert(asyncProxy != null, nameof(asyncProxy) + " != null");
                foreach (var handler in entry.Value.Where(x => asyncType.IsInstanceOfType(x)))
                {
                    asyncTasks.Add((Task)asyncProxy.Invoke(handler, new object[] { eventInfo }));
                }

                var syncType = typeof(IHandle<>).MakeGenericType(entry.Key);
                var syncProxy = syncType.GetMethod("Handle");
                Debug.Assert(syncProxy != null, nameof(syncProxy) + " != null");
                foreach (var handler in entry.Value.Where(x => syncType.IsInstanceOfType(x)))
                {
                    try
                    {
                        syncProxy.Invoke(handler, new object[] {eventInfo});
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }
            }

            try
            {
                Task.WaitAll(asyncTasks.ToArray());
            }
            catch (AggregateException e)
            {
                exceptions.AddRange(e.InnerExceptions);
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        private void AddHandler(Type eventType, IHandle handler)
        {
            var handlers = GetHandlersForSpecificType(eventType);
            if (!handlers.Contains(handler))
            {
                handlers.Add(handler);
            }
        }

        public IEnumerable<IHandle> GetRegisteredHandlers<T>()
        {
            return GetHandlersForSpecificType(typeof(T));
        }

        private List<IHandle> GetHandlersForSpecificType(Type t)
        {
            if (_registrationList.TryGetValue(t, out var output)) return output;

            output = new List<IHandle>();
            _registrationList.Add(t, output);
            return output;
        }

        private IEnumerable<KeyValuePair<Type, List<IHandle>>> GetHandlersAssignableToType(Type t)
        {
            return _registrationList.Where(x => x.Key.IsAssignableFrom(t));
        }
    }
}
