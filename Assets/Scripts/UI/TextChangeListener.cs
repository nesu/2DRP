using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Event;
using Assets.Scripts.Event.UI;
using Assets.Scripts.Manager;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TextChangeListener : MonoBehaviour
    {
        public string Key;
        private TextMeshProUGUI _text;

        void OnEnable()
        {
            _text = GetComponent<TextMeshProUGUI>();
            //Debug.Log("Listening for text changes on " + name);
            EventManager.AddListener("OnTextChange", OnTextChange);
        }

        void OnDisable()
        {
            EventManager.RemoveListeners("OnTextChange");
        }

        private void OnTextChange(GameEvent uevent)
        {
            if (uevent is TextChangeEvent ev)
            {
                //Debug.Log("Key: " + ev.Key + " Text: " + ev.Text);
                if (Key == ev.Key)
                {
                    _text.text = ev.Text;
                }
            }
        }
    }
}
