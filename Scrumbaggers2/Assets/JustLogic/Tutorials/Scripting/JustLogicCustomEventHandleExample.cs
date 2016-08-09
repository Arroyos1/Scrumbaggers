using System;
using System.Collections.Generic;
using JustLogic.Core;
using MyCustomEvents;
using UnityEngine;

[ExecuteInEditMode]
public class JustLogicCustomEventHandleExample : JustLogicEventHandlerLite
{
    void MyCustomEvent1()
    {
        Trigger(new MyCustomEvent1());
    }

    void MyCustomEvent2(int x)
    {
        Trigger(new MyCustomEvent2(), x);
    }
}

namespace MyCustomEvents
{
    public struct MyCustomEvent1 : IEventDescription
    {
        public string Name { get { return "MyCustomEvent1"; } }
        public EventDescriptionArguments Arguments { get { return EventDescriptionArguments.None; } }
        public string RequiredEventHandler { get { return typeof(JustLogicCustomEventHandleExample).FullName; } }
    }

    public struct MyCustomEvent2 : IEventDescription
    {
        public string Name { get { return "MyCustomEvent2"; } }
        readonly static EventDescriptionArguments Args = new EventDescriptionArguments(
            new[]
            {
                // one int argument
                new KeyValuePair<string, Type>("arg", typeof(int))
            });
        public EventDescriptionArguments Arguments { get { return Args; } }
        public string RequiredEventHandler { get { return typeof(JustLogicCustomEventHandleExample).FullName; } }
    }
}