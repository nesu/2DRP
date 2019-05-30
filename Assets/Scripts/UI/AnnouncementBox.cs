using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Event;
using Assets.Scripts.Event.UI;
using Assets.Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class AnnouncementBox : MonoBehaviour
    {
        private TextMeshProUGUI _message;
        private Image _image;

        void Start()
        {
            _message = GetComponentInChildren<TextMeshProUGUI>();
            _image = GetComponent<Image>();
            //EventManager.TriggerEvent("OnAnnouncementChanged", new OnAnnouncementChangedEvent("VEIKIA", 5f));
        }


        void OnEnable()
        {
            EventManager.AddListener("OnAnnouncementChanged", OnAnnouncementChanged);
            EventManager.AddListener("OnAnnouncementDisabled", OnAnnouncementDisabled);
        }

        private void OnAnnouncementDisabled(GameEvent _)
        {
            SetEnabled(false);
        }


        void OnDisable()
        {
            EventManager.RemoveListeners("OnAnnouncementChanged");
        }


        private void OnAnnouncementChanged(GameEvent uevent)
        {
            if (uevent is OnAnnouncementChangedEvent announcement)
            {
                SetEnabled(true);
                _message.text = announcement.Message;

                if (announcement.Timeout > 0)
                {
                    StartCoroutine(CloseAfter(announcement.Timeout));
                }
            }
        }


        private IEnumerator CloseAfter(float timeout)
        {
            yield return new WaitForSeconds(timeout);
            SetEnabled(false);
        }


        private void SetEnabled(bool value)
        {
            _image.enabled = value;
            _message.enabled = value;
        }
    }
}
