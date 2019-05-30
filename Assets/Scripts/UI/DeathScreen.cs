using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class DeathScreen : MonoBehaviour
    {
        public TextMeshProUGUI KillCountText;
        public TextMeshProUGUI WaveCountText;
        public TextMeshProUGUI HighscoreText;

        void Start()
        {
            int lrkc = PlayerPrefs.GetInt("LastRoundKC");
            int lrwc = PlayerPrefs.GetInt("LastRoundWC");

            int hskc = Highscore.GetHighscoreKillCount();
            int hswc = Highscore.GetHighscoreWaveCount();
            

            if (hskc < lrkc || hswc < lrwc)
            {
                HighscoreText.gameObject.SetActive(true);
                if (hskc < lrkc)
                    PlayerPrefs.SetInt("KillCount", lrkc);
                if (hswc < lrwc)
                    PlayerPrefs.SetInt("WaveCount", lrwc);
            }

            KillCountText.text = "Kill count: " + lrkc;
            WaveCountText.text = "Waves cleared: " + lrwc;
        }

        public void OnDeathScreenExit()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
