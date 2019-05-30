using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Event
{
    public class StatIncrementEvent : GameEvent
    {
        public string Key;

        public StatIncrementEvent(string key)
        {
            Key = key;
        }
    }
}
