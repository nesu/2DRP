using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Event;
using Assets.Scripts.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Observer
{
    public class PlatformsCollisionObserver : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D entity)
        {
            if (entity.gameObject.tag == "Player")
            {
                EventManager.TriggerEvent("OnPlayerEnterPlatform");
            }

            if (entity.gameObject.tag == "Monster")
            {
                EventManager.TriggerEvent("OnMonsterEnterPlatform", new PlatformCollisionEvent(entity.gameObject, true));
            }
        }

        void OnCollisionExit2D(Collision2D entity)
        {
            if (entity.gameObject.tag == "Player")
            {
                EventManager.TriggerEvent("OnPlayerExitPlatform");
            }

            if (entity.gameObject.tag == "Monster")
            {
                EventManager.TriggerEvent("OnMonsterExitPlatform", new PlatformCollisionEvent(entity.gameObject, false));
            }
        }
    }
}
