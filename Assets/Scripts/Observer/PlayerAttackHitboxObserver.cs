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

namespace Assets.Scripts.Observer
{
    public class PlayerAttackHitboxObserver : MonoBehaviour
    {
        [Header("Max number of enemies to hurt")]
        public int MaxHitCount = 3;

        private Animator _animator;
        private float _directed;

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            _directed = transform.position.x >= 0 ? 1.0f : -1.0f;
            _animator.SetFloat("Direction", _directed);
        }


        void OnTriggerEnter2D(Collider2D entity)
        {
            if (entity.gameObject.tag == "Monster")
            {
                var enemy = entity.GetComponent<Enemy>();
                if (enemy != null)
                {
                    //enemy.Hit(50);
                }
            }
        }
    }
}
