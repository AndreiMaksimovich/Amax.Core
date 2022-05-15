// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;

#endregion

namespace Amax {

    // ----------------------------------------------------------------------------------------
    // Event Base

    public class EventBusBaseEvent: EventArgs {}

    public delegate void EventBusCallback<E>(E data) where E:EventBusBaseEvent;

    public interface IEventBusListenerBase {}

    public interface IEventBusListener<E> : IEventBusListenerBase
    {
        void OnEvent(E data);
    }
    
    // ----------------------------------------------------------------------------------------

    public static class EventBus 
    {

        private static readonly Dictionary<Type, List<IEventBusListenerBase>> Listeners = new Dictionary<Type, List<IEventBusListenerBase>>();
        private static readonly Dictionary<Type, List<Delegate>> Callbacks = new Dictionary<Type, List<Delegate>>();

        // ------------------------------------------

        public static void AddListener<Type>(IEventBusListener<Type> listener) where Type:EventBusBaseEvent
        {
            var type = typeof(Type);
            if (!Listeners.ContainsKey(type)) Listeners.Add(type, new List<IEventBusListenerBase>());
            var list = Listeners[type];
            if (!list.Contains(listener)) list.Add(listener);
        }

        public static void RemoveListener<Type>(IEventBusListener<Type> listener) where Type : EventBusBaseEvent 
        {
            var type = typeof(Type);
            if (!Listeners.ContainsKey(type)) return;
            Listeners[type].Remove(listener);
        }

        // ------------------------------------------

        public static void AddCallback<Type>(EventBusCallback<Type> callback) where Type : EventBusBaseEvent 
        {
            var type = typeof(Type);
            if (!Callbacks.ContainsKey(type)) Callbacks.Add(type, new List<Delegate>());
            var list = Callbacks[type];
            if (!list.Contains(callback)) list.Add(callback);
        }

        public static void RemoveCallback<Type>(EventBusCallback<Type> callback) where Type : EventBusBaseEvent 
        {
            var type = typeof(Type);
            if (!Callbacks.ContainsKey(type)) return;
            Callbacks[type].Remove(callback);
        }

        // ------------------------------------------

        public static void Clear<Type>() where Type:EventBusBaseEvent 
        {
            Listeners.Remove(typeof(Type));
            Callbacks.Remove(typeof(Type));
        }   

        public static void Clear() 
        {
            Listeners.Clear();
            Callbacks.Clear();
        }

        // ------------------------------------------

        public static void RaiseEvent<Type>(Type eventData) where Type:EventBusBaseEvent 
        {
            var type = eventData.GetType();
            if (Listeners.ContainsKey(type)) 
            {
                foreach (var listener in Listeners[type]) 
                {
                    (listener as IEventBusListener<Type>).OnEvent(eventData);
                }
            }
            if (Callbacks.ContainsKey(type)) 
            {
                foreach (var callback in Callbacks[type]) 
                {
                    (callback as EventBusCallback<Type>).Invoke(eventData);
                }
            }
        }

        // ------------------------------------------
        
    }


}