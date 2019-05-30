using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Highscore
    {
        public int Enemies;
        public int Waves;

        public static int GetHighscoreKillCount()
        {
            return PlayerPrefs.GetInt("KillCount");
        }


        public static int GetHighscoreWaveCount()
        {
            return PlayerPrefs.GetInt("WaveCount");
        }
    }
}
