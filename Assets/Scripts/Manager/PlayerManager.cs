using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Assets.Scripts.Event;
using Assets.Scripts.Event.UI;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace Assets.Scripts.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        enum InputCommand
        {
            Jump, Prone, Ability, None
        }

        [Header("Player settings")]
        public float Speed = 1f;
        public int JumpForce = 10;
        public List<Ability> Abilities;


        [Header("Initial player stats")]
        public int BaseLevel = 1;
        public float BaseHealth = 130f;
        public float BaseEnergyRegen = 11f;
        public float BaseAttackDamage = 32f;

 
        [Header("HUD components")]
        public Slider HealthSlider;
        public Slider EnergySlider;

        [HideInInspector]
        public int Points = 0;

        [HideInInspector]
        public int KillCount = 0;

        [HideInInspector]
        public int WaveCount = 0;


        private int _level;
        private float _health;
        private float _energy;
        private float _speed;

        private Rigidbody2D _rigidbody;
        private Animator _animator;

        private bool _isGrounded;
        private Vector2 _movement = Vector2.zero;
        private float _horizontal = -1.0f;
        private InputCommand _command = InputCommand.None;

        private bool _teleporting;
        private bool _isDashing = false;


        void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();

            InitializePlayer();
            InvokeRepeating("RegenerateEnergy", 0f, 1f);
        }


        private bool IsPlayerFalling => _rigidbody.velocity.y < 0;
        private bool IsPlayerRising => _rigidbody.velocity.y > 0;
        private float AdaptiveSpeed => IsPlayerFalling || IsPlayerRising ? Speed * 0.7f : Speed;


        void InitializePlayer()
        {
            _level = BaseLevel;
            _health = GetMaxHealth();
            _speed = Speed;

            HealthSlider.value = GetHealthPercentage();

            _energy = 100f;
            EnergySlider.value = 1;
        }


        void OnEnable()
        {
           EventManager.AddListener("OnPlayerEnterPlatform", (GameEvent _) =>
           {
               _isGrounded = true;
               _speed = Speed;
           });

           EventManager.AddListener("OnPlayerExitPlatform", (GameEvent _) => _isGrounded = false);
           EventManager.AddListener("OnPlayerHit", OnPlayerHit);
           EventManager.AddListener("OnPlayerDamaged", OnPlayerDamaged);

           EventManager.AddListener("OnStatIncrement", OnStatIncrement);
        }

        private void OnPlayerDamaged(GameEvent uevent)
        {
            if (!(uevent is OnPlayerDamagedEvent damaged))
                return;

            _health -= damaged.Damage;
            if (damaged.Knockback > 0) {
                _rigidbody.AddForce(new Vector2(2f * damaged.Knockback * (damaged.Sender.transform.localScale.x * -1), damaged.Knockback * 2f), ForceMode2D.Impulse);
            }

            if (_health <= 0)
            {
                EventManager.TriggerEvent("OnPlayerDeath", new PlayerDeathEvent(KillCount, WaveCount));
            }
        }

        private void OnPlayerHit(GameEvent uevent)
        {
            if (uevent is OnPlayerHitEvent hit)
            {
                _health -= hit.Source.GetDamage();
                //Debug.Log("Left " + _health + " of " + GetMaxHealth() + " HP.");
                _rigidbody.AddForce(new Vector2(2f * hit.Source.GetDirectionToTarget(), 1.5f), ForceMode2D.Impulse);
                if (_health <= 0)
                {
                    EventManager.TriggerEvent("OnPlayerDeath", new PlayerDeathEvent(KillCount, WaveCount));
                }
            }
        }


        void OnDisable()
        {
           EventManager.RemoveListeners("OnPlayerEnterPlatform");
           EventManager.RemoveListeners("OnPlayerExitPlatform");
           EventManager.RemoveListeners("OnPlayerHit");
           EventManager.RemoveListeners("OnStatIncrement");
        }


        void Update()
        {
            HealthSlider.value = GetHealthPercentage();
            EnergySlider.value = _energy / 100f;

            _movement = Vector2.zero;
            _movement.x = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !IsPlayerRising)
            {
                _command = InputCommand.Jump;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && _command == InputCommand.None && !_isDashing && _isGrounded && _energy >= 20)
            {
                _energy -= 20f;
                _command = InputCommand.Ability;
                _isDashing = true;
                return;
            }

            foreach (var it in Abilities)
            {
                /**
                 * Paspaudžia mygtuką 
                 */
                if (Input.GetKeyDown(it.Key) && _command != InputCommand.Ability && it.EnergyCost <= _energy)
                {
                    StartCoroutine(Ability(it)); 
                    break;
                }
            }
        }

        private IEnumerator Dash()
        {
            _isDashing = true;
            Physics2D.IgnoreLayerCollision(8, 10);

            _animator.SetInteger("AbilityType", 3);
            _rigidbody.velocity = new Vector2(_horizontal * 20f, 0);

            yield return new WaitForSeconds(0.22f);
            
            _isDashing = false;
            Physics2D.IgnoreLayerCollision(8, 10, false);
            _animator.SetInteger("AbilityType", 0);
            _rigidbody.velocity = Vector2.zero;
            _command = InputCommand.None;
        }


        void FixedUpdate()
        {
            if (_isDashing)
            {
                StartCoroutine(Dash());
                return;
            }


            if (_movement == Vector2.zero)
            {
                _animator.SetBool("IsMoving", false);
            }
            else if (!IsPlayerRising && _command != InputCommand.Jump)
            {
                _animator.SetBool("IsMoving", true);
                _animator.SetFloat("Horizontal", _movement.x);

                if (_isGrounded) {
                    _movement.y = 0;
                }

                _horizontal = _movement.x;
                _rigidbody.MovePosition(_rigidbody.position + _movement * _speed * Time.fixedDeltaTime);
                //EventManager.TriggerEvent("OnSoundPlay", new SoundPlayEvent("Step"));
            }

            if (_command == InputCommand.Jump)
            {
                _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
                _animator.SetBool("GroundLeap", true);
                _command = InputCommand.None;
                _speed *= 0.7f;
            }

            if (IsPlayerFalling)
            {
                _animator.SetBool("GroundLeap", false);
                _animator.SetBool("IsFalling", true);
            }
            else if(_animator.GetBool("IsFalling"))
            {
                _animator.SetBool("IsFalling", false);
            }
        }

        private void OnStatIncrement(GameEvent uevent)
        {
            if (Points <= 0)
                return;

            if (uevent is StatIncrementEvent increment)
            {
                switch (increment.Key)
                {
                    case "Attack":
                        BaseAttackDamage += BaseAttackDamage * 0.1f;
                        EventManager.TriggerEvent("OnTextChange", new TextChangeEvent("Attack", "Attack: " + Math.Round(BaseAttackDamage, 2)));
                        break;
                    case "Stamina":
                        BaseEnergyRegen += BaseEnergyRegen * 0.1f;
                        BaseHealth += BaseHealth * 0.1f;
                        EventManager.TriggerEvent("OnTextChange", new TextChangeEvent("Stamina", "Stamina: " + Math.Round(BaseHealth, 2)));
                        break;
                    case "Agility":
                        if (Speed > 10f)
                        {
                            Speed = 10f;
                            return;
                        }

                        Speed += Speed * 0.05f;
                        EventManager.TriggerEvent("OnTextChange", new TextChangeEvent("Agility", "Agility: " + Math.Round(Speed, 2)));
                        break;
                }

                Points -= 1;
                EventManager.TriggerEvent("OnTextChange", new TextChangeEvent("Points", "Points: " + Points));

                InitializePlayer();
            }
        }


        public float GetHealthPercentage()
        {
            if (_health <= 0)
            {
                return 0;
            }

            return _health / GetMaxHealth();
        }


        public float GetMaxHealth()
        {
            return BaseHealth + _level * BaseHealth * 0.3f;
        }


        private void RegenerateEnergy()
        {
            if (_energy < 100f)
            {
                _energy += BaseEnergyRegen;
            }
            else if(_energy > 100f)
            {
                _energy = 100f;
            }
        }


        private IEnumerator Ability(Ability ability)
        {
            _command = InputCommand.Ability;
            EventManager.TriggerEvent("OnAbilityCast", new AbilityEvent(ability, _horizontal));
            _energy -= ability.EnergyCost;

            yield return new WaitForSeconds(ability.GetAdjustedAnimationLength(_horizontal));

            EventManager.TriggerEvent("OnAbilityEnd");
            _command = InputCommand.None;
        }

        public bool IsGrounded()
        {
            return _isGrounded;
        }
    }
}