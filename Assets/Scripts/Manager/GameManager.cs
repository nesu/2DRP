using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Event.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Time = UnityEngine.Time;

namespace Assets.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        public Image Overlay;

        private bool _paused;

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Scene: " + scene.name);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TriggerGameState();
            }
        }

        public void TriggerGameState()
        {
            if (_paused)
            {
                _paused = false;
                // Disable pause menu here.
                Time.timeScale = 1;
                Overlay.gameObject.SetActive(false);
                //EventManager.TriggerEvent("OnAnnouncementDisabled");
            }
            else
            {
                _paused = true;
                Overlay.gameObject.SetActive(true);

                Time.timeScale = 0;
                //EventManager.TriggerEvent("OnAnnouncementChanged", new OnAnnouncementChangedEvent("Game paused", 0f));
            }
        }


        public void OnGameClose()
        {
            TriggerGameState();
            SceneManager.LoadScene("Menu");
        }

        public void OnGameExit()
        {
            Application.Quit();
        }
    }
}
