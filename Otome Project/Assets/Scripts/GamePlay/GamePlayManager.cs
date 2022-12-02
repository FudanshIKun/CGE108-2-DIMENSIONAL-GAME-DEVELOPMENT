using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Otome.UI;

namespace Otome.GamePlay
{
    public class GamePlayManager : MonoBehaviour
    {
        #region Singleton

        public static GamePlayManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Detect more than one GamePlayManager!");
            }

            Instance = this;
        }

        #endregion

        #region UI management

        [SerializeField] GamePlayUI mainUI;

        #endregion

        #region Story Management

        [SerializeField] StoryScene mainStory;

        private void ReadStory()
        {
            
        }

        #endregion

        #region GamePlayManagement

        

        #endregion
        
        private void Start()
        {
            
        }
    }
}
