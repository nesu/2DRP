using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Event
{
    public class SoundPlayEvent : GameEvent
    {
        public string Sound;

        public SoundPlayEvent(string sound)
        {
            Sound = sound;
        }
    }
}
