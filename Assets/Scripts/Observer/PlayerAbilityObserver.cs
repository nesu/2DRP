using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Observer
{
    public class PlayerAbilityObserver : MonoBehaviour
    {
        public Collider2D PlayerLeftAOE;
        public Collider2D PlayerRightAOE;
        public Collider2D PlayerSwordAOE;

        // Cooldown for all abilities.
        public float BaseCD;
        private float _cooldown;

        public bool IsAnimating = false;
        public bool AbilityQ = false;

        // Animator in PlayerObject.
        private Animator _animator;
        // Animator of player's left AOE. Used by special effects of abilities.
        private Animator _leftAnimator;
        // Animator of player's right AOE. Used by special effects of abilities.
        private Animator _rightAnimator;
        

        void Start()
        {
            _animator = GetComponent<Animator>();
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !IsAnimating)
            {
                if (Math.Abs(_cooldown) < 0.01)
                {
                    _cooldown = BaseCD;
                    AbilityCast(1);
                }
                else
                {
                    _cooldown -= Time.deltaTime;
                }
            }
        }


        public void AbilityImpact()
        {
            int type = _animator.GetInteger("AbilityType");
        }


        public void AbilityAnimationEnd()
        {
            _animator.SetInteger("AbilityType", 0);
            IsAnimating = false;
        }


        private void AbilityCast(int type)
        {
            IsAnimating = true;
            //_animator.SetInteger("AbilityType", type);
            if (type == 1)
            {
                _animator.Play("Ability: Q");
            }
        }
    }
}