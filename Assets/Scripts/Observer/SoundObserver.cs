using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Event;
using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Observer
{
    public class SoundObserver : MonoBehaviour
    {
        public AudioClip[] Effects;

        private AudioSource _source;
       

        public Dictionary<string, AudioClip> _effectd;


        void Start()
        {
            _source = GetComponent<AudioSource>();
            _effectd = new Dictionary<string, AudioClip>();

            foreach (var it in Effects)
            {
                _effectd[it.name] = it;
                Debug.Log(it.name + " audio clip loaded.");
            }
        }


        void OnEnable()
        {
            EventManager.AddListener("OnSoundPlay", OnSoundPlay);
        }


        void OnDisable()
        {
            EventManager.RemoveListeners("OnSoundPlay");
        }
        

        private void OnSoundPlay(GameEvent uevent)
        {
            if (uevent is SoundPlayEvent sound)
            {
                if (_effectd.TryGetValue(sound.Sound, out var clip))
                {
                    _source.clip = clip;
                    _source.loop = false;
                    _source.Play();
                }
            }
        }
    }
}
