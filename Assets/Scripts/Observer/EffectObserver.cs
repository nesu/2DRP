using System.Collections;
using Assets.Scripts.Event;
using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Observer
{
    [RequireComponent(typeof(Animator))]
    public class EffectObserver : MonoBehaviour
    {
        public string Owner;
        public string Activator;
        public float Length;

        private Animator _animator;


        void Start()
        {
            _animator = GetComponent<Animator>();
        }


        void OnEnable()
        {
            EventManager.AddListener("OnEffectFired", OnEffectFired);
        }


        void OnDisable()
        {
            EventManager.RemoveListeners("OnEffectFired");
        }


        private void OnEffectFired(GameEvent uevent)
        {
            if (!(uevent is OnEffectEvent effect))
                return;

            if (effect.Owner == Owner && effect.Activator == Activator)
            {
                var posit = effect.DeferredPosition();
                if (posit != Vector2.zero) {
                    transform.position = posit;
                }

                _animator.SetBool(Activator, true);
                StartCoroutine(EffectEnd());
            }
        }

        private IEnumerator EffectEnd()
        {
            yield return new WaitForSeconds(Length);
            _animator.SetBool(Activator, false);
        }
    }
}