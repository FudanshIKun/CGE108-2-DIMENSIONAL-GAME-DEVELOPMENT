using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Otome.Core
{
    public class DataManager : MonoBehaviour
    {
        [Header("File Storage Config")] 
        [SerializeField] private string fileName;
        
        #region Singleton
            public static DataManager Instance { get; private set; }
            private void Awake()
            {
                if (Instance != null)
                {
                    Debug.Log("Detect more than one DataManager!");
                }

                Instance = this;
            }

        #endregion

        private GameData _gameData;
        private List<IDataManager> _dataObjects;
        private FileDataHandler _dataHandler;
        
        public void NewGame()
        {
            _gameData = new GameData();
        }

        public void LoadGame()
        {
            // Load any saved data from a file using the data handler
            _gameData = _dataHandler.Load();
            
            // if no data can be loaded, initialize to a new game
            if (_gameData == null)
            {
                Debug.Log("Can't Detect any saved GameData. Initializing data to default.");
                NewGame();
            }
            
            // push the loaded data to all other script that need it
            foreach (IDataManager dataObj in _dataObjects)
            {
                dataObj.LoadData(_gameData);
            }
        }

        public void SaveGame()
        {
            // pass the data to other script so they can update it
            foreach (IDataManager dataObj in _dataObjects)
            {
                dataObj.SaveData(ref _gameData);
            }

            // save that data to a file using data handler
            _dataHandler.Save(_gameData);
        }
        
        private List<IDataManager> FindAllDataObjects()
        {
            IEnumerable<IDataManager> dataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataManager>();
            return new List<IDataManager>(dataObjects);
        }

        void Start()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            _dataObjects = FindAllDataObjects();
            if (GameManager.Instance.currentScene == GameManager.SceneList.MainMenu)
            {
                LoadGame();
            }
            else
            {
                NewGame();
            }
        }

        private void OnDestroy()
        {
            Debug.Log("OnDestroy Call");
            SaveGame();
        }
    }
}
