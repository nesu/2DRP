using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Manager;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public abstract class EnemyBehaviour : MonoBehaviour
    {
        protected enum Direction
        {
            Left = -1,
            Right = 1
        }

        protected Monster Monster;
        protected PlayerManager Target;
        protected Animator Animator;
        protected Rigidbody2D Rigidbody;

        protected void Initialize()
        {
            Monster = GetComponent<Monster>();
            Target = FindObjectOfType<PlayerManager>();
            Animator = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody2D>();

            Debug.Log("Behaviour for " + Monster.Named + " loaded.");
        }


        protected Direction TargetDirection()
        {
            var difference = Target.transform.position - transform.position;
            return (difference / difference.magnitude).x > 0 ? Direction.Right : Direction.Left;
        }
    }
}
