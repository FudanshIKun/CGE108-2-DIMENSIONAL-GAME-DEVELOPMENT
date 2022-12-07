using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Otome.Core
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        #endregion

        #region Scene Management

        public enum SceneList
        {
            MainMenu,
            LevelMenu,
            Level01,
            Level02
        }
        public SceneList currentScene;

        public void LoadScene(SceneList targetScene)
        {
            gameState = GameState.Loading;
        }

        #endregion

        #region Overview of Game system

        public enum GameState
        {
            InMenu,
            Loading,
            InGame
        }
        public GameState gameState;

        #endregion

        void Start()
        {
            switch (SceneManager.GetActiveScene().ToString())
            {
                case var value when value == SceneList.MainMenu.ToString() :
                    gameState = GameState.InMenu;
                    break;
                case var value when value == SceneList.LevelMenu.ToString() :
                    gameState = GameState.InMenu;
                    break;
                case var value when value == SceneList.Level01.ToString() :
                    gameState = GameState.InGame;
                    break;
                case var value when value == SceneList.Level02.ToString() :
                    gameState = GameState.InGame;
                    break;
            }
        }

        void Update()
        {
            Debug.Log(gameState);
        }
    }
}
