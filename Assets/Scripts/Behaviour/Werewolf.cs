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

namespace Assets.Scripts.Behaviour
{
    [RequireComponent(typeof(Monster))]
    public class Werewolf : EnemyBehaviour
    {
        private ContactFilter2D _filter;
        private readonly Collider2D[] _colliders = new Collider2D[1];

        private Direction _last = 0;
        private bool _attacking = false;
        private bool _rage = false;
        private float _dazed = 0.5f;

        int Colliders => Monster.Hitbox.OverlapCollider(_filter, _colliders);


        void Start()
        {
            Initialize();

            _filter = new ContactFilter2D();
            _filter.SetLayerMask(Monster.Attackable);
        }

        void Update()
        {
            if (!Monster.CanBehave())
                return;

            if (Monster.CalculateHealthPercentage() <= 0.5f && !_rage)
            {
                _rage = true;
                StartCoroutine(ActiveRage());
                return;
            }

            if (_rage && Animator.GetBool("ActiveRage"))
                return;

            if (Colliders == 0)
            {
                Direction directed = TargetDirection();

                if (directed != _last)
                {
                    _last = directed;
                    Animator.SetBool("IsMoving", false);
                    StartCoroutine(Monster.Stun(Monster.Speed * _dazed));
                    return;
                }

                var scale = transform.localScale;
                scale.x = (float) directed * -1;
                transform.localScale = scale;

                Animator.SetBool("IsMoving", true);
                Rigidbody.MovePosition(Rigidbody.position + new Vector2((float) directed, 0) * Monster.Speed * Time.deltaTime);
            }
            else
            {
                Animator.SetBool("IsMoving", false);
                if (!_attacking) {
                    StartCoroutine(Attack());
                }
            }
        }

        private IEnumerator ActiveRage()
        {
            Animator.SetBool("ActiveRage", true);
            yield return new WaitForSeconds(1f);
            Animator.SetBool("ActiveRage", false);

            Monster.Speed *= 1.75f;
            _dazed = 0.15f;
        }

        IEnumerator Attack()
        {
            if (!Monster.CanBehave())
                yield break;

            _attacking = true;
            Animator.SetBool("IsAttacking", true);
            yield return new WaitForSeconds(0.5f);
            // Laiko langas žaidėjui išvengti atakos. Jam praėjus tikrinama ar žaidėjas vis dar atakos zonoj.
            if (Colliders > 0)
            {
                if (!Monster.CanBehave())
                {
                    // Monstro ataka nutraukiama jeigu jis tuo metu buvo nužudytas arba apsvaigintas.
                    _attacking = false;
                    Animator.SetBool("IsAttacking", false);
                    Animator.SetBool("IsMoving", false);
                    yield break;
                }

                Debug.Log("Player damaged.");
                EventManager.TriggerEvent("OnEffectFired", new OnEffectEvent(Monster.Named, "IsActive", () => Target.transform.position));
                EventManager.TriggerEvent("OnPlayerDamaged", new OnPlayerDamagedEvent(Monster, Monster.CalculateDamage(), 1.7f));
            }

            yield return new WaitForSeconds(0.5f);

            Animator.SetBool("IsAttacking", false);
            Animator.SetBool("IsMoving", false);
            _attacking = false;
        }
    }
}
