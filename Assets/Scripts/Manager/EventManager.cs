using System;
using System.Collections.Generic;
using Assets.Scripts.Event;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Manager
{
    public class EventManager : MonoBehaviour
    {
        private static EventManager _self;
        private readonly Dictionary<string, Action<GameEvent>> _events;


        public EventManager()
        {
            _events = new Dictionary<string, Action<GameEvent>>();
            _self = this;
        }


        public static void AddListener(string named, Action<GameEvent> listener)
        {
            if (_self._events.TryGetValue(named, out var uevent))
            {
                uevent += listener;
                _self._events[named] = uevent;
            }
            else
            {
                uevent += listener;
                _self._events.Add(named, uevent);
            }
        }


        public static void RemoveListener(string named, Action<GameEvent> listener)
        {
            if (_self == null)
                return;

            if (_self._events.TryGetValue(named, out var uevent))
            {
                uevent -= listener;
                _self._events[named] = uevent;
            }
        }


        public static void RemoveListeners(string named)
        {
            if (_self == null)
                return;

            if (_self._events.TryGetValue(named, out var uevent))
            {
                _self._events[named] = null;
            }
        }


        public static void TriggerEvent(string named)
        {
            if (_self._events.TryGetValue(named, out var uevent))
            {
                uevent.Invoke(null);
            }
        }


        public static void TriggerEvent(string named, GameEvent pevent)
        {
            if (_self._events.TryGetValue(named, out var uevent))
            {
                uevent.Invoke(pevent);
            }
        }
    }
}