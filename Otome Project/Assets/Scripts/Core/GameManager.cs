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

        #endregion

        #region GamePlay

        public enum GameState
        {
            Playing,
            Paused
        }

        #endregion
    }
}
