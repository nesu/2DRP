using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Wave
    {
        public string Label;
        public Monster Monster;
        public int SpawnCount;
        public float SpawnRate;
    }
}
