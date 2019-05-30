using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Event.UI
{
    public class OnAnnouncementChangedEvent : GameEvent
    {
        public string Message;
        public float Timeout;

        public OnAnnouncementChangedEvent(string message, float timeout)
        {
            Message = message;
            Timeout = timeout;
        }
    }
}
