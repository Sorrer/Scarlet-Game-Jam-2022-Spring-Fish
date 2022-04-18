using Game.Systems.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.UI
{
    public class TitleScreenManager : MonoBehaviour 
    {
        [Header("Dependencies")]
        [SerializeField]
        private string gameplayLevel;

        public void Play()
        {
            SceneManager.LoadScene(gameplayLevel);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}