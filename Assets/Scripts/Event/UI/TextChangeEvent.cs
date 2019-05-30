using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Event.UI
{
    public class TextChangeEvent : GameEvent
    {
        public string Text;
        public string Key;

        public TextChangeEvent(string key, string text)
        {
            Key = key;
            Text = text;
        }
    }
}
