using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Event;
using Assets.Scripts.Event.UI;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.Scripts.Manager
{
    public class WaveManager : MonoBehaviour
    {
        public enum SpawnState
        {
            Spawning,
            Waiting,
            Counting
        }

        public GameObject Spawns;
        public List<Wave> Waves;
        public Image Overlay;

        public float TimeBetweenWaves = 4f;


        private bool _enabled = false;
        private int _wave = 0;
        private float _waveTimer = 0;
        private float _rescanTimer = 0f;
        private SpawnState _state = SpawnState.Counting;
        private List<Transform> _spawns;
        private PlayerManager _player;
        private int _iteration = 1;
        private Random _random = new Random();


        void Start()
        {
            _waveTimer = TimeBetweenWaves;
            _spawns = Spawns.GetComponentsInChildren<Transform>().ToList();
            _player = FindObjectOfType<PlayerManager>();

            if (Waves.Count > 0)
            {
                Debug.Log("WaveManager has defined " + Waves.Count + " waves. Checking...");
                _enabled = true;

                foreach (var wave in Waves)
                {
                    if (wave.Monster == null || wave.SpawnCount == 0 || wave.SpawnRate < 0.001)
                    {
                        Debug.LogWarning("Wave " + wave.Label + " has incorrect configuration.");
                        Debug.LogWarning("Disabling wave manager.");
                        _enabled = false;
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("WaveManager has no defined waves. Disabling...");
            }
        }


        void OnEnable()
        {
            EventManager.AddListener("OnNextWave", OnNextWave);
        }


        void OnDisable()
        {
            EventManager.RemoveListeners("OnNextWave");
        }


        void Update()
        {
            if (!_enabled)
                return;

            if (_state == SpawnState.Waiting)
            {
                if (IsWaveCleared())
                {
                    OnWaveEnd();
                }

                return;
            }

            if (_waveTimer <= 0)
            {
                if (_state != SpawnState.Spawning)
                {
                    StartCoroutine(SpawnWave(Waves[_wave]));
                }
            }
            else
            {
                _waveTimer -= Time.deltaTime;
            }
        }


        private IEnumerator SpawnWave(Wave wave)
        {
            _state = SpawnState.Spawning;
            for (int it = 0; it < wave.SpawnCount; it++)
            {
                SpawnEnemy(wave.Monster);
                yield return new WaitForSeconds(1f / wave.SpawnRate);
            }

            _state = SpawnState.Waiting;
            yield break;
        }


        private void SpawnEnemy(Monster enemy)
        {
            Debug.Log("Spawning enemy...");
            var spawn = _spawns.ElementAt(_random.Next(_spawns.Count));
            //enemy.Level += _iteration;

            var spawned = Instantiate(enemy.gameObject, new Vector2(spawn.position.x, spawn.position.y), enemy.transform.rotation);
            spawned.gameObject.SetActive(true);
        }


        private void OnWaveEnd()
        {
            _enabled = false;
            Overlay.gameObject.SetActive(true);
            _player.Points += 3 * Waves[_wave].SpawnCount;
            _player.KillCount += Waves[_wave].SpawnCount;
            _player.WaveCount += 1;

            _state = SpawnState.Counting;
            _waveTimer = TimeBetweenWaves;
            _wave += 1;
            if (_wave >= Waves.Count)
            {
                Debug.Log("Resetting wave cycle and increasing difficulty.");
                _wave = 0;
                _iteration += 1;
            }

            EventManager.TriggerEvent("OnWaveEnd", new WaveEvent(_wave, _iteration, _player));
        }


        private void OnNextWave(GameEvent _)
        {
            _enabled = true;
            Overlay.gameObject.SetActive(false);
        }


        private bool IsWaveCleared()
        {
            _rescanTimer -= Time.deltaTime;
            if (_rescanTimer <= 0)
            {
                _rescanTimer = 1f;
                return GameObject.FindGameObjectWithTag("Monster") == null;
            }

            return false;
        }
    }
}
