using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Otome.Core
{
    public class DataManager : MonoBehaviour
    {
        [Header("File Storage Config")] 
        [SerializeField] private string fileName;

        public List<Character> allCharacter = new List<Character>();

        #region Singleton
            public static DataManager Instance { get; private set; }
            private void Awake()
            {
                if (Instance != null)
                {
                    Debug.Log("Detect more than one DataManager! Destroying the newest one");
                    Destroy(gameObject);
                    return;
                }

                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            }

        #endregion

        private GameData _gameData;
        private List<IDataManager> _dataObjects;
        private FileDataHandler _dataHandler;
        
        public void NewGame()
        {
            _gameData = new GameData();
            foreach (Character person in allCharacter)
            {
                person.heartOwned = 0;
            }
        }

        public void LoadGame()
        {
            // Load any saved data from a file using the data handler
            _gameData = _dataHandler.Load();

            // if no data can be loaded, don't continue
            if (_gameData == null)
            {
                Debug.LogWarning("Can't Detect any saved GameData. " +
                                 "A NewGame needs to be started before data can be loaded");
                return;
            }

            // push the loaded data to all other script that need it
            foreach (IDataManager dataObj in _dataObjects)
            {
                dataObj.LoadData(_gameData);
            }
        }

        public void SaveGame()
        {
            if (_gameData == null)
            {
                Debug.LogWarning("No data was found. A NewGame needs to be " +
                                 "started before data can be saved");
                return;
            }
            Debug.Log("SaveGame Called");
            
            // pass the data to other script so they can update it
            foreach (IDataManager dataObj in _dataObjects)
            {
                dataObj.SaveData(ref _gameData);
            }

            // save that data to a file using data handler
            _dataHandler.Save(_gameData);
        }

        public bool CheckGameDataExist()
        {
            if (_gameData != null)
            {
                return true;
            }
            
            return false;
        }

        public bool CheckCompleatedScene(GameManager.SceneList sceneTargetKey, bool value)
        {
            bool actualValue;
            if (!_gameData.playedLevels.TryGetValue(sceneTargetKey.ToString(), out actualValue))
            {
                return false;
            }
            return actualValue == value;
        }

        public bool CheckPassedLevel(GameManager.SceneList sceneTargetKey)
        {
            bool actualValue;
            if (!_gameData.passedLevels.TryGetValue(sceneTargetKey.ToString(),out actualValue))
            {
                return false;
            }

            return actualValue;
        }

        private List<IDataManager> FindAllDataObjects()
        {
            IEnumerable<IDataManager> dataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataManager>();
            return new List<IDataManager>(dataObjects);
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _dataObjects = FindAllDataObjects();
            if (GameManager.Instance.currentScene == GameManager.SceneList.MainMenu )
            {
                Debug.Log("Game Loaded at MainMenu scene");
                LoadGame();
            }
        }

        public void OnSceneUnloaded(Scene scene)
        {
            Debug.Log("On SceneUnloaded");
            if (GameManager.Instance.gameState == GameManager.GameState.InGame)
            {
                Debug.Log("Game Saved when exit the game scene");
                SaveGame();
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }
    }
}
