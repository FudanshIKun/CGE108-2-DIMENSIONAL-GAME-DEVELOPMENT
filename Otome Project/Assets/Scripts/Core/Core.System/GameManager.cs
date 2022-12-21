using System;
using System.Collections.Generic;
using UnityEngine;
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
                    Instance.currentScene = currentScene;
                    Instance.gameState = gameState;
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
                SceneManager.LoadSceneAsync(targetScene.ToString());
            }

        #endregion

        #region Overview of Game system

            public enum GameState
            {
                InMenu,
                InGame
            }
            public GameState gameState;

        #endregion

        void Start()
        {
            
        }

        void Update()
        {
            Debug.Log("CurrenScene : " + currentScene);
        }
    }
}
