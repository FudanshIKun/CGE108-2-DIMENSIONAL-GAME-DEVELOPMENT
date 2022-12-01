using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otome.Core
{
    public class DataManager : MonoBehaviour
    {
        private GameData _gameData;
        public static DataManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Detect more than one DataManager");
            }

            Instance = this;
        }

        public void NewGame()
        {
            this._gameData = new GameData();
        }

        public void LoadGame()
        {
            if (this._gameData == null)
            {
                Debug.Log("Detect gamedata was saveed. Initializing data to default.");
                NewGame();
            }
        }

        public void SaveGame()
        {
            
        }
    }
}
