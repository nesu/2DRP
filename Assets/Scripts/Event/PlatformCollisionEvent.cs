using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Event
{
    public class PlatformCollisionEvent : GameEvent
    {
        public GameObject Sender;
        public bool IsGrounded;

        public PlatformCollisionEvent(GameObject sender, bool isGrounded)
        {
            Sender = sender;
            IsGrounded = isGrounded;
        }
    }
}
