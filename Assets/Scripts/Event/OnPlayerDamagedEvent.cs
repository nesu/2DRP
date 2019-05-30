using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Model;

namespace Assets.Scripts.Event
{
    public class OnPlayerDamagedEvent : GameEvent
    {
        public float Damage;
        public Monster Sender;
        public float Knockback;

        public OnPlayerDamagedEvent(Monster sender, float damage, float knockback = 0f)
        {
            Damage = damage;
            Sender = sender;
            Knockback = knockback;
        }
    }
}
