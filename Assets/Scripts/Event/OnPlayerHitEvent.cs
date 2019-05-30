using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Model;

namespace Assets.Scripts.Event
{
    public class OnPlayerHitEvent : GameEvent
    {
        public Enemy Source;

        public OnPlayerHitEvent(Enemy enemy)
        {
            Source = enemy;
        }
    }
}
