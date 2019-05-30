using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Event;
using Assets.Scripts.Manager;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Observer
{
    public class MonsterObserver : MonoBehaviour
    {
        void OnEnable()
        {
            EventManager.AddListener("OnMonsterEnterPlatform", MonsterPlatformObserver);
            EventManager.AddListener("OnMonsterExitPlatform", MonsterPlatformObserver);
        }


        void OnDisable()
        {
            EventManager.RemoveListeners("OnMonsterEnterPlatform");
            EventManager.RemoveListeners("OnMonsterExitPlatform");
        }


        private void MonsterPlatformObserver(GameEvent uevent)
        {
            if (!(uevent is PlatformCollisionEvent e))
                return;

            if (e.Sender != null)
            {
                var monster = e.Sender.GetComponent<Monster>();
                if (monster != null) {
                    monster.IsGrounded = e.IsGrounded;
                }
            }
        }
    }
}
