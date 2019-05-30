using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Ability
    {
        public int Type;
        public float EnergyCost = 0;
        public float DamageMultiplier;

        public KeyCode Key;
        public AnimationClip AnimationLeft;
        public AnimationClip AnimationRight;
        public float HitboxSize;
        public Transform HitboxLeft;
        public Transform HitboxRight;

        public float GetAdjustedAnimationLength(float horizontal)
        {
            return horizontal > 0 ? AnimationRight.length : AnimationLeft.length;
        }

        public Vector2 GetAdjustedHitbox(float horizontal)
        {
            return horizontal < 0 ? HitboxLeft.position : HitboxRight.position;
        }
    }
}
