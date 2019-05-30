using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Model;

namespace Assets.Scripts.Event
{
    public class OnEnemyDeathEvent : GameEvent
    {
        public Enemy Sender;

        public OnEnemyDeathEvent(Enemy enemy)
        {
            Sender = enemy;
        }
    }
}
