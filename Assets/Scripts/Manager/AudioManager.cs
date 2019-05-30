using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Event;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public AudioClip[] Playlist;

        private AudioManager _self;
        private AudioSource _audio;
        private int _cursor = 0;
        private bool _paused = false;

        public AudioManager Get
        {
            get
            {
                if (_self == null)
                {
                    _self = new AudioManager();
                    return _self;
                }

                return _self;
            }
        }

        void Start()
        {
            _audio = GetComponent<AudioSource>();
            _audio.volume = 0.74f;
        }


        void Awake()
        {
            DontDestroyOnLoad(this);
            var managers = FindObjectsOfType<AudioManager>();
            if (managers.Length != 1)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }


        void Update()
        {
            if (!_audio.isPlaying && !_paused)
            {
                if (_cursor > Playlist.Length)
                    _cursor = 0;

                _audio.clip = Playlist[_cursor];
                _audio.Play();
                _cursor += 1;
            }
        }


        void OnEnable()
        {
            EventManager.AddListener("OnMusicPause", OnMusicPause);
            EventManager.AddListener("OnMusicResume", OnMusicResume);
        }

        private void OnMusicResume(GameEvent obj)
        {
            _paused = false;
            _audio.UnPause();
        }


        void OnDisable()
        {
            EventManager.RemoveListeners("OnMusicPause");
            EventManager.RemoveListeners("OnMusicResume");
        }


        private void OnMusicPause(GameEvent obj)
        {
            _paused = true;
            _audio.Pause();
        }
    }
}
