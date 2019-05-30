using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Event
{
    public class OnEffectEvent : GameEvent
    {
        public string Owner;
        public string Activator;
        public Func<Vector2> DeferredPosition;

        public OnEffectEvent(string owner, string activator, Func<Vector2> position)
        {
            Owner = owner;
            Activator = activator;
            DeferredPosition = position;
        }
    }
}
