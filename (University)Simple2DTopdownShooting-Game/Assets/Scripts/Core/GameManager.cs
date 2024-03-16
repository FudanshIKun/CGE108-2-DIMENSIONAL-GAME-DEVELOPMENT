using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleLight.Core
{
    // Singleton Pattern
    public class GameManager : MonoBehaviour
    {
        Animator SceneAnimator;
        #region Singleton Pattern

        public static GameManager Instance;
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

        public void CollectComponents(){
            
        }
        
        void Awake() {
            Singleton();            
            CollectComponents();
        }

        void SceneLoad(Scene targetScene)
        {
            
        }

        void SceneLoadAnimation()
        {
            
        }
    }
}
