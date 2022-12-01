using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Otome.Core;

namespace Otome.UI
{
    public class MainMenu : MonoBehaviour
    {
        public Button startButton;
        public Button loadButton;
        public Button optionButton;
        public Button albumButton;
        public Button exitButton;
    
        void AddUIFunction(){
            startButton.clicked += StartButtonPressed;
            loadButton.clicked += LoadButtonPressed;
            optionButton.clicked += OptionButtonPressed;
            albumButton.clicked += AlbumButtonPressed;
            exitButton.clicked += ExitButtonPressed;
        }

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            startButton = root.Q<Button>("StartButton");
            loadButton = root.Q<Button>("LoadButton");
            optionButton = root.Q<Button>("OptionButton");
            albumButton = root.Q<Button>("AlbumButton");
            exitButton = root.Q<Button>("ExitButton");

            AddUIFunction();

        }

    
        void StartButtonPressed(){
            
        }

        void LoadButtonPressed(){
            SceneManager.LoadScene(GameManager.SceneList.LevelMenu.ToString());
        }

        void OptionButtonPressed(){

        }

        void AlbumButtonPressed(){

        }

        void ExitButtonPressed(){

        }
    }
}
