using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Otome.UI
{
    public class MainMenu : MonoBehaviour
    {
        UIManagement _ui;
        #region MainMenu Panel

        VisualElement MainMenuElements;
        Button newGameButton;
        Button loadButton;
        Button optionButton;
        Button diaryButton;
        Button exitButton;

        void NewGameButtonPressed(){
            _ui.SwitchPanel(MainMenuElements, NewGameElement);
        }
        void LoadButtonPressed(){
            _ui.LoadScene();
        }
        void OptionButtonPressed(){
            _ui.SwitchPanel(MainMenuElements, OptionElement);
        }
        void DiaryButtonPressed(){
            _ui.SwitchPanel(MainMenuElements,diaryElement );
        }
        void ExitButtonPressed(){
            _ui.SwitchPanel(MainMenuElements,ExitElement);
        }

        #endregion

        #region NewGame Panel

        VisualElement NewGameElement;
        Button newgame_YesButton;
        Button newgame_NoButton;

        void NewGame_YesPressed()
        {
            Debug.Log("Reset Game");
        }
        void NewGame_NoPressed()
        {
            _ui.SwitchPanel(NewGameElement, MainMenuElements);
        }

        #endregion

        #region Option Panel

        VisualElement OptionElement;
        

        #endregion

        #region Diary Panel

        VisualElement diaryElement;

        #endregion

        #region Exit panel

        VisualElement ExitElement;
        Button exit_YesButton;
        Button exit_NoButton;

        void Exit_YesPressed()
        {
            Debug.Log("Leave the Game");
        }
        void Exit_NoPressed()
        {
            _ui.SwitchPanel(ExitElement,MainMenuElements);
        }

        #endregion

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _ui = GetComponent<UIManagement>();

            #region root.Q

            MainMenuElements = root.Q<VisualElement>("MainMenuElement");
            newGameButton = root.Q<Button>("StartButton");
            loadButton = root.Q<Button>("LoadButton");
            optionButton = root.Q<Button>("OptionButton");
            diaryButton = root.Q<Button>("DiaryButton");
            exitButton = root.Q<Button>("ExitButton");
            NewGameElement = root.Q<VisualElement>("NewGameElement");
            newgame_YesButton = root.Q<Button>("NewGame_YesButton");
            newgame_NoButton = root.Q<Button>("NewGame_NoButton");
            OptionElement = root.Q<VisualElement>("OptionElement");
            diaryElement = root.Q<VisualElement>("DiaryElement");
            ExitElement = root.Q<VisualElement>("ExitElement");
            exit_YesButton = root.Q<Button>("Exit_YesButton");
            exit_NoButton = root.Q<Button>("Exit_NoButton");

            #endregion

            AddUIFunction();
        }
        
        void AddUIFunction(){
            newGameButton.clicked += NewGameButtonPressed;
            loadButton.clicked += LoadButtonPressed;
            optionButton.clicked += OptionButtonPressed;
            diaryButton.clicked += DiaryButtonPressed;
            exitButton.clicked += ExitButtonPressed;
            newgame_YesButton.clicked += NewGame_YesPressed;
            newgame_NoButton.clicked += NewGame_NoPressed;
            exit_YesButton.clicked += Exit_YesPressed;
            exit_NoButton.clicked += Exit_NoPressed;
        }
    }
}
