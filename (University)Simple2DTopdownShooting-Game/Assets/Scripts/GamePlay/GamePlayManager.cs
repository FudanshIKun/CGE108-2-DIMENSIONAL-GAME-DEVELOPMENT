using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleLight.GamePlay
{
    public class GamePlayManager : MonoBehaviour
    { 
        
         #region Singleton Pattern

        public static GamePlayManager Instance;
        private void Singleton(){
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
        
        public GameObject mainCharacter;
        public List<Monster> existMonster = new List<Monster>();

        void Awake()
        {
            Singleton();
        }

        private void Start()
        {
            mainCharacter = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            Debug.Log(existMonster.Count().ToString());
            foreach (var i in existMonster)
            {
                if (i.hasDied)
                {
                    existMonster.Remove(i);
                }
            }
            SimpleEndGame();
            
        }

        void SimpleEndGame()
        {
            if (existMonster.Count == 0 || existMonster == null)
            {
                Debug.Log("The game has ended");
                if (Input.GetKeyDown(KeyCode.Backslash))
                {
                    Application.Quit();
                }
            }
        }
    }
}
