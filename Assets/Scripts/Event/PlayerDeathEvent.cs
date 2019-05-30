using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Event
{
    public class PlayerDeathEvent : GameEvent
    {
        public int KillCount;
        public int WaveCount;

        public PlayerDeathEvent(int kc, int wc)
        {
            KillCount = kc;
            WaveCount = wc;
        }
    }
}
