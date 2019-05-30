using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class BaseMenu : MonoBehaviour
    {
        public void OnGameStart()
        {
            SceneManager.LoadScene("Arena");
        }

        public void OnGameExit()
        {
            Application.Quit();
        }
    }
}
