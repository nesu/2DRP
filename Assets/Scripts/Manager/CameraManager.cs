using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class CameraManager : MonoBehaviour
    {
        public Transform Target;
        public float MaxCameraX;
        public float MaxCameraY;

        [HideInInspector]
        public float MinCameraX;

        [HideInInspector]
        public float MinCameraY;

        private Vector2 _velocity = Vector2.zero;

        void Start()
        {
            MinCameraX = MaxCameraX * -1;
            MinCameraY = MaxCameraY * -1;
        }


        void LateUpdate()
        {
            //transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
            float x = Mathf.SmoothDamp(transform.position.x, Target.transform.position.x, ref _velocity.x, 0.05f);
            float y = Mathf.SmoothDamp(transform.position.y, Target.transform.position.y, ref _velocity.y, 0.05f);

            transform.position = new Vector3(x, y, transform.position.z);

            transform.position = Bounded;
        }


        private Vector3 Bounded => new Vector3(
            Mathf.Clamp(transform.position.x, MinCameraX, MaxCameraX),
            Mathf.Clamp(transform.position.y, MinCameraY, MaxCameraY),
            transform.position.z
        );
    }
}
