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
            switch (SceneManager.GetActiveScene().name)
            {
                case "MainMenu" :
                    gameState = GameState.InMenu;
                    currentScene = SceneList.MainMenu;
                    break;
                case "LevelMenu" :
                    gameState = GameState.InMenu;
                    currentScene = SceneList.LevelMenu;
                    break;
                case "Level01" :
                    gameState = GameState.InGame;
                    currentScene = SceneList.Level01;
                    break;
                case "Level02" :
                    gameState = GameState.InGame;
                    currentScene = SceneList.Level02;
                    break;
            }
        }

        void Update()
        {
            //Debug.Log("GameState : " + gameState);
            //Debug.Log("CurrentScene : " + currentScene);
        }
    }
}
