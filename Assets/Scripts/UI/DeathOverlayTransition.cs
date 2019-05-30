using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Event;
using Assets.Scripts.Manager;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class DeathOverlayTransition : MonoBehaviour
    {
        private Image _overlay;
        private Animator _animator;


        void Start()
        {
            _overlay = GetComponent<Image>();
            _animator = GetComponent<Animator>();
            _overlay.enabled = false;

        }


        void OnEnable()
        {
            Debug.Log("Enabled.");
            EventManager.AddListener("OnPlayerDeath", OnPlayerDeath);
        }


        void OnDisable()
        {
            Debug.Log("Disabled.");
            EventManager.RemoveListeners("OnPlayerDeath");
        }


        private void OnPlayerDeath(GameEvent uevent)
        {
            if (uevent is PlayerDeathEvent death)
            {
                _overlay.enabled = true;
                StartCoroutine(Transition());
                PlayerPrefs.SetInt("LastRoundKC", death.KillCount);
                PlayerPrefs.SetInt("LastRoundWC", death.WaveCount);
            }
        }


        private IEnumerator Transition()
        {
            _animator.SetTrigger("OnDeath");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Death");
        }
    }
}
