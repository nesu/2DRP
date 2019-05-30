using System.Collections;
using System.Linq;
using Assets.Scripts.Event;
using Assets.Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Model
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Monster : MonoBehaviour
    {
        public int Level;
        public string Named;
        public int BaseHealth;
        public int BaseDamage;
        public float Speed;
        public GameObject HealthCanvas;
        public LayerMask Attackable;
        public AnimationClip DeathAnimation;
        public float SliderOffsetY;

        [HideInInspector]
        public Collider2D Hitbox;

        [HideInInspector]
        public bool IsGrounded = false;


        private Animator _animator;
        private Rigidbody2D _rigidbody;

        private bool _isStunned = false;

        private float _health;
        private GameObject _hCanvas;
        private Slider _hSlider;


        void Start()
        {
            Hitbox = GetComponentsInChildren<Collider2D>().First(it => it.gameObject.tag == "EntityHitbox");

            if (Hitbox == null) {
                Debug.LogWarning("Cannot find hitbox collider for " + Named);
            }

            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _health = CalculateMaxHealth();

            var rendererf = GetComponent<SpriteRenderer>();

            _hCanvas = Instantiate(HealthCanvas, new Vector3(0, SliderOffsetY, 0), Quaternion.identity);
            _hCanvas.transform.SetParent(transform, false);
            _hCanvas.SetActive(true);



            //var canvas = Instantiate(HealthCanvas, gameObject.transform, true);
            //canvas.SetActive(true);

            //canvas.transform.position = Vector3.zero;
            //Debug.Log(canvas.transform.position);
            
            _hSlider = _hCanvas.GetComponentInChildren<Slider>();
            _hSlider.value = 1;
        }


        void OnEnable()
        {
        }


        public void Damage(int damage)
        {
            _health = _health - damage;
            _hSlider.value = CalculateHealthPercentage();
            StartCoroutine(Knockback(2f));

            if (_health <= 0)
            {
                // Įvykis naudojamas WaveManager klasėje.
                EventManager.TriggerEvent("OnMonsterDeath", new OnEnemyDeathEvent(null));
                StartCoroutine(Death());
            }
        }


        private IEnumerator Death()
        {
            if (DeathAnimation != null)
            {
                _animator.SetTrigger("OnDeath");
                yield return new WaitForSeconds(DeathAnimation.length);
                Destroy(gameObject);
            }
        }


        public IEnumerator Knockback(float power)
        {
            IsGrounded = false;
            _rigidbody.velocity = new Vector2(2f * transform.localScale.x, 2.3f);
            yield return new WaitForSeconds(0.4f);
            _rigidbody.velocity = Vector2.zero;
            IsGrounded = true;
        }


        public IEnumerator Stun(float seconds)
        {
            _isStunned = true;
            yield return new WaitForSeconds(seconds);
            _isStunned = false;
        }


        public IEnumerator Confuse()
        {
            yield return new WaitForSeconds(10f);
        }


        public float CalculateDamage()
        {
            return BaseDamage + Level * (0.17f + BaseDamage);
        }


        public float CalculateMaxHealth()
        {
            return BaseHealth * ((Level + 1) / 0.5f);
        }


        public float CalculateHealthPercentage()
        {
            if (_health <= 0)
                return 0;

            return _health / CalculateMaxHealth();
        }


        public bool CanBehave()
        {
            return _health > 0 && !_isStunned && IsGrounded;
        }
    }
}
