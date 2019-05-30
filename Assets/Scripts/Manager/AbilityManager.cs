using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Event;
using Assets.Scripts.Model;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Manager
{
    public class AbilityManager : MonoBehaviour
    {
        public LayerMask Attackable;
        public Animator FXAnimator;

        private PlayerManager _player;
        private Animator _animator;

        void Start()
        {
            _animator = GetComponentInParent<Animator>();
            _player = GetComponentInParent<PlayerManager>();
        }


        void OnEnable()
        {
            EventManager.AddListener("OnAbilityCast", OnAbilityCast);
            EventManager.AddListener("OnAbilityEnd", OnAbilityEnd);
        }


        void OnDisable()
        {
            EventManager.RemoveListeners("OnAbilityCast");
            EventManager.RemoveListeners("OnAbilityEnd");
        }


        private void OnAbilityEnd(GameEvent _)
        {
            _animator.SetInteger("AbilityType", 0);
            FXAnimator.SetBool("IsActive", false);
        }


        private void OnAbilityCast(GameEvent uevent)
        {
            if (uevent is AbilityEvent cast)
            {
                Ability ability = cast.Ability;
                Collider2D[] monsters = Physics2D.OverlapCircleAll(ability.GetAdjustedHitbox(cast.Directed), ability.HitboxSize, Attackable);

                Random rand = new Random();
                int slash = rand.Next(1, 3);

                //EventManager.TriggerEvent("OnSoundPlay", new SoundPlayEvent("Slash_" + slash));

                _animator.SetInteger("AbilityType", ability.Type);
                // Kardo kirčio spec. efektas.
                FXAnimator.SetFloat("Horizontal", cast.Directed);
                FXAnimator.SetBool("IsActive", true);


                foreach (var it in monsters)
                {
                    var monster = it.GetComponent<Monster>();
                    if (monster != null)
                    {
                        var damage = _player.BaseAttackDamage * ability.DamageMultiplier;
                        monster.Damage((int) damage);
                    }


                    //if (enemy != null)
                    //{
                     //   var damage = _player.BaseAttackDamage * ability.DamageMultiplier;
                      //  //Debug.Log("Player damage: " + damage);
                       // enemy.Hit((int)damage, cast.Directed, 1f);
                   // }
                }
            }
        }
    }
}
