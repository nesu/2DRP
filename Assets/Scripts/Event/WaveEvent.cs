using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Manager;

namespace Assets.Scripts.Event
{
    public class WaveEvent : GameEvent
    {
        public int Wave;
        public int Difficulty;
        public PlayerManager Sender;

        public WaveEvent(int wave, int difficulty, PlayerManager sender)
        {
            Wave = wave;
            Difficulty = difficulty;
            Sender = sender;
        }
    }
}
