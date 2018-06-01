# SimpleEventHub
A simple to use Event Hub for .NET that allows you to publish and subscribe to events of any type.

When a single instance of `EventHub` is distributed throughout your application using a DI container, this can enable strong decoupling
between different areas of your code.

# Nuget
This package can be used by installing it from Nuget

![Nuget](https://img.shields.io/nuget/v/SimpleEventHub.svg)

# Quick Start
SimpleEventHub provides a lightweight class which allows you to subscribe to, and publish events of any type. The implementation is based off
of the EventAggregator ni the Caliburn.Micro framework.

## Registering
To subscribe to an event, you must "Register" an object that implements `IHandle<T>`, where `T` is the type of event you want to handle.

    class SyncBaseCountingHandler : IHandle<BaseEvent>
    {
        public int HandleCount { get; set; }
        public void Handle(BaseEvent eventInfo)
        {
            HandleCount++;
        }
    }
    
To then register this handler, you should pass an instance of the handler to the `Register` method on an 'EventHub` instance

    var hub = new EventHub();
    var handler = new SyncBaseCountingHandler();
    
    hub.Register(handler);
    
## Publish an Event
To publish an event, pass an object of any type to the `AddEvent` method to the `EventHub` instance

    var hub = new EventHub();
    var handler = new SyncBaseCountingHandler();
    hub.Register(handler);
    
    hub.AddEvent(new BaseEvent());
    Console.Log(handler.HandleCount); 
    
    //Outputs: 1
    
## Covariance
The SimpleEventHub supports covariance of event types. This means that if a handler handles a type, that the event type is derived from,
the handler will still receive that event. This enables you to, for example, handle all events of any type, by implementing `IHandle<object>`

    var hub = new EventHub();
    var handler = new SyncBaseCountingHandler();
    hub.Register(handler);
    
    hub.AddEvent(new DerivedEvent());
    Console.Log(handler.HandleCount); 
    
    //Outputs: 1
