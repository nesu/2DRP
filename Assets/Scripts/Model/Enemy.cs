using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Event;
using Assets.Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable InconsistentNaming
namespace Assets.Scripts.Model
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        public enum CrowdControl
        {
            None,
            Stun,
            Slow
        }

        public string Named;
        public int BaseHealth;
        public int BaseDamage;
        public int Level;
        public float HealthBarOffset = 0.3f;
        public GameObject HealthCanvas;
        public LayerMask Attackable;

        [HideInInspector]
        public CrowdControl CrowdControlState;

        private float _maxHealth;
        private float _health;

        private GameObject _HUDHealthCanvas;
        private Slider _HUDHealthSlider;
        private RectTransform _HUDHealthRect;
        private Animator _animator;
        private Rigidbody2D _rigidbody;
        private Dictionary<string, AnimationClip> _animations;
        private EnemyBehaviour _behaviour;
        private Transform _target;

        void Start()
        {
            _maxHealth = BaseHealth * ((Level + 1) / 0.5f);
            _health = _maxHealth;

            _HUDHealthCanvas = Instantiate(HealthCanvas);
            _HUDHealthCanvas.transform.SetParent(gameObject.transform);
            _HUDHealthRect = _HUDHealthCanvas.GetComponent<RectTransform>();
            _HUDHealthRect.localPosition = new Vector2(0, HealthBarOffset);
            _HUDHealthCanvas.SetActive(true);

            _HUDHealthSlider = _HUDHealthCanvas.GetComponentInChildren<Slider>();
            _HUDHealthSlider.value = GetHealthPercentage();

            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _behaviour = GetComponent<EnemyBehaviour>();

            if (_animator == null)
            {
                Debug.LogError("Monster " + Named + " has no animator component.");
                Application.Quit(1);
                return;
            }

            if (_behaviour == null)
            {
                Debug.LogWarning("Monster " + Named + " has no defined behaviour.");
            }

            var animations = _animator.runtimeAnimatorController.animationClips;
            _animations = new Dictionary<string, AnimationClip>();
            Debug.Log("Monster " + Named + " has " + animations.Length + " animations.");

            foreach (var it in animations)
            {
                _animations.Add(it.name, it);
            }

            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }


        void Update()
        {
            _HUDHealthSlider.value = GetHealthPercentage();
           // _behaviour.Behaviour();
        }


        public float GetDamage()
        {
            return BaseDamage + Level * (0.17f * BaseDamage);
        }


        public float GetMaxHealth()
        {
            return _maxHealth;
        }


        public float GetHealthPercentage()
        {
            if (_health <= 0)
            {
                return 0;
            }

            return _health / _maxHealth;
        }


        public void Hit(int damage, float horizontal, float knockback)
        {
            _health = _health - damage;

            if (knockback > 0)
            {
                var adjusted = horizontal < 0 ? knockback * -1.0f : knockback;
                Debug.Log("Knocking back " + Named + " with force of " + adjusted);
                StartCoroutine(Knockback(adjusted));
            }


            if (_health <= 0)
            {
                Debug.Log("Monster (" + Named + ") died.");
                EventManager.TriggerEvent("OnEnemyDeath", new OnEnemyDeathEvent(this));
                StartCoroutine(OnDeath());
            }
        }


        private IEnumerator OnDeath()
        {
            if (_animations.TryGetValue("DeathLeft", out AnimationClip death))
            {
                _animator.SetTrigger("OnDeath");
                yield return new WaitForSeconds(death.length);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Monster " + Named + " has no defined death animation.");
                yield break;
            }
            
            yield break;
        }


        public IEnumerator Knockback(float knockback)
        {
            var y = _rigidbody.position.y;

            //_rigidbody.bodyType = RigidbodyType2D.Dynamic;
            //_rigidbody.isKinematic = false;

            _rigidbody.AddForce(new Vector2(knockback, 1.5f), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.3f);

            _rigidbody.velocity = Vector2.zero;
            _rigidbody.transform.position = new Vector2(_rigidbody.position.x, y);
            //_rigidbody.bodyType = RigidbodyType2D.Kinematic;
            //_rigidbody.isKinematic = true;
            StartCoroutine(Stun(0.2f));
            yield break;
        }


        public IEnumerator Stun(float seconds)
        {
            if (seconds <= 0)
                yield break;

            CrowdControlState = CrowdControl.Stun;
            yield return new WaitForSeconds(seconds);
            CrowdControlState = CrowdControl.None;
            yield break;
        }


        public bool CanAct()
        {
            return _health > 0 && CrowdControlState != CrowdControl.Stun;
        }


        public AnimationClip GetAnimation(string named)
        {
            if (_animations.TryGetValue(named, out AnimationClip ac))
            {
                return ac;
            }

            Debug.Log("Animation " + named + " was not found in enemy's " + Named + " animation dictionary.");
            return null;
        }


        public float GetAnimationLength(string named)
        {
            if (_animations.TryGetValue(named, out AnimationClip ac))
            {
                return ac.length;
            }

            Debug.Log("Animation " + named + " was not found in enemy's " + Named + " animation dictionary.");
            return -1.0f;
        }


        public Transform GetTarget()
        {
            return _target;
        }


        public float GetDirectionToTarget()
        {
            var heading = _target.position - transform.position;
            return (heading / heading.magnitude).x > 0 ? 1 : -1;
        }
       
    }
}
