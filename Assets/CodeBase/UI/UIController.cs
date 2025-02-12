using System;
using CodeBase.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject hud;
        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;

        private void Start()
        {
            ShowHudScreen();
        }

        private void ShowHudScreen()
        {
            winScreen.SetActive(false);
            loseScreen.SetActive(false);
            
            hud.SetActive(true);
            
            Time.timeScale = 1f;
        }

        public void ShowWinScreen()
        {
            hud.SetActive(false);
            loseScreen.SetActive(false);
            
            winScreen.SetActive(true);
            
            Time.timeScale = 0f;
        }

        public void ShowLoseScreen()
        {
            hud.SetActive(false);
            winScreen.SetActive(false);
            
            loseScreen.SetActive(true);
            
            Time.timeScale = 0f;
        }
    }
}