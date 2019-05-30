using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Model;
using UnityEngine;
using TMPro;


namespace Assets.Scripts.UI
{
    public class HighscoreMenu : MonoBehaviour
    {
        public TextMeshProUGUI KillCountText;
        public TextMeshProUGUI WaveCountText;

        void Start()
        {
            KillCountText.text = "Kill count: " + Highscore.GetHighscoreKillCount();
            WaveCountText.text = "Waves cleared: " + Highscore.GetHighscoreWaveCount();
        }
    }
}
