using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Event;
using Assets.Scripts.Event.UI;
using Assets.Scripts.Manager;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.UI
{
    public class WaveOverlay : MonoBehaviour
    {
        void OnEnable()
        {
            Debug.Log("WaveOverlay activated.");
            EventManager.AddListener("OnWaveEnd", OnWaveEnd);
        }


        void OnDisable()
        {
            Debug.Log("WaveOverlay disabled.");
            EventManager.RemoveListeners("OnWaveEnd");
        }


        private void OnWaveEnd(GameEvent uevent)
        {
            Debug.Log("OnWaveEnd called.");

            if (uevent is WaveEvent wave)
            {
                var sender = wave.Sender;
                EventManager.TriggerEvent("OnTextChange", new TextChangeEvent("Attack", "Attack: " + Math.Round(sender.BaseAttackDamage, 2)));
                EventManager.TriggerEvent("OnTextChange", new TextChangeEvent("Stamina", "Stamina: " + Math.Round(sender.BaseHealth, 2)));
                EventManager.TriggerEvent("OnTextChange", new TextChangeEvent("Agility", "Agility: " + Math.Round(sender.Speed, 2)));
                EventManager.TriggerEvent("OnTextChange", new TextChangeEvent("Points", "Points: " + sender.Points));
            }
        }

        public void IncrementAttack()
        {
            Debug.Log("Player added one point in attack.");
            EventManager.TriggerEvent("OnStatIncrement", new StatIncrementEvent("Attack"));
        }

        public void IncrementStamina()
        {
            Debug.Log("Player added one point in stamina.");
            EventManager.TriggerEvent("OnStatIncrement", new StatIncrementEvent("Stamina"));
        }

        public void IncrementAgility()
        {
            Debug.Log("Player added one point in agility.");
            EventManager.TriggerEvent("OnStatIncrement", new StatIncrementEvent("Agility"));
        }

        public void NextWave()
        {
            EventManager.TriggerEvent("OnNextWave");
            
        }
    }
}
