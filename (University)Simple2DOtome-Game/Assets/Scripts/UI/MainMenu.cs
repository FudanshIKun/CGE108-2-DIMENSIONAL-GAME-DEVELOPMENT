using System;
using UnityEngine;
using UnityEngine.UIElements;
using Otome.Core;

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
                if (DataManager.Instance.CheckGameDataExist())
                {
                    _ui.SwitchPanel(MainMenuElements, NewGameElement);
                }
                else
                {
                    // Create a new game - which will initialize our game data & save it
                    DataManager.Instance.NewGame();
                    DataManager.Instance.SaveGame();
                    // OnSceneUnloaded() in DataManager
                    GameManager.Instance.LoadScene(GameManager.SceneList.LevelMenu);
                    newGameButton.pickingMode = PickingMode.Ignore;
                }
            }
            
            void LoadButtonPressed(){
                if (DataManager.Instance.CheckGameDataExist())
                {
                    if (DataManager.Instance.CheckCompleatedScene(
                            GameManager.SceneList.Level01, true) &&
                        DataManager.Instance.CheckCompleatedScene(
                            GameManager.SceneList.Level02, true))
                    {
                        _ui.SwitchPanel(MainMenuElements, loadGameElement);
                    }
                    else
                    {
                        GameManager.Instance.LoadScene(GameManager.SceneList.LevelMenu);
                        loadButton.pickingMode = PickingMode.Ignore;
                    }
                }
                else
                {
                    _ui.SwitchPanel(MainMenuElements, warningElement);
                }
            }
            
            void OptionButtonPressed(){
                _ui.SwitchPanel(MainMenuElements, settingPanel);
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
                // Create a new game - which will initialize our game data & save it
                DataManager.Instance.NewGame();
                DataManager.Instance.SaveGame();
                // OnSceneUnloaded() in DataManager
                GameManager.Instance.LoadScene(GameManager.SceneList.LevelMenu);
                newgame_YesButton.pickingMode = PickingMode.Ignore;
            }
            void NewGame_NoPressed()
            {
                _ui.SwitchPanel(NewGameElement, MainMenuElements);
            }

        #endregion

        #region LoadGame Panel

        VisualElement loadGameElement;
        Button loadGameYes;
        Button loadGameNo;

        void LoadGame_YesPressed()
        {
            // Create a new game - which will initialize our game data & save it
            DataManager.Instance.NewGame();
            DataManager.Instance.SaveGame();
            // OnSceneUnloaded() in DataManager
            GameManager.Instance.LoadScene(GameManager.SceneList.LevelMenu);
            loadGameYes.pickingMode = PickingMode.Ignore;
        }

        void LoadGame_NoPressed()
        {
            _ui.SwitchPanel(loadGameElement, MainMenuElements);
        }
        
        #endregion

        #region Warning Panel

        VisualElement warningElement;
        Button warningOk;

        void WarningOkPressed()
        {
            _ui.SwitchPanel(warningElement, MainMenuElements);
        }

        #endregion

        #region Option Panel

            VisualElement settingPanel;
            Button settingBack;

            void SettingBackPressed()
            {
                _ui.SwitchPanel(settingPanel, MainMenuElements);
            }

        #endregion

        #region Exit panel

            VisualElement ExitElement;
            Button exit_YesButton;
            Button exit_NoButton;

            void Exit_YesPressed()
            {
                Application.Quit();
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
                exitButton = root.Q<Button>("ExitButton");
                NewGameElement = root.Q<VisualElement>("NewGameElement");
                newgame_YesButton = root.Q<Button>("NewGame_YesButton");
                newgame_NoButton = root.Q<Button>("NewGame_NoButton");
                loadGameElement = root.Q<VisualElement>("LoadGameElement");
                loadGameYes = root.Q<Button>("LoadGame_YesButton");
                loadGameNo = root.Q<Button>("LoadGame_NoButton");
                warningElement = root.Q<VisualElement>("WarningElement");
                warningOk = root.Q<Button>("Warning_OkButton");
                settingPanel = root.Q<VisualElement>("SettingPanel");
                settingBack = root.Q<Button>("SettingBack");
                ExitElement = root.Q<VisualElement>("ExitElement");
                exit_YesButton = root.Q<Button>("Exit_YesButton");
                exit_NoButton = root.Q<Button>("Exit_NoButton");

            #endregion

            AddUIFunction();
            PreparedUI();
        }

        void PreparedUI()
        {
            NewGameElement.style.display = DisplayStyle.None;
            loadGameElement.style.display = DisplayStyle.None;
            settingPanel.style.display = DisplayStyle.None;
            warningElement.style.display = DisplayStyle.None;
        }
        
        void AddUIFunction(){
            newGameButton.clicked += NewGameButtonPressed;
            loadButton.clicked += LoadButtonPressed;
            optionButton.clicked += OptionButtonPressed;
            settingBack.clicked += SettingBackPressed;
            exitButton.clicked += ExitButtonPressed;
            newgame_YesButton.clicked += NewGame_YesPressed;
            newgame_NoButton.clicked += NewGame_NoPressed;
            exit_YesButton.clicked += Exit_YesPressed;
            exit_NoButton.clicked += Exit_NoPressed;
            loadGameYes.clicked += LoadGame_YesPressed;
            loadGameNo.clicked += LoadGame_NoPressed;
            warningOk.clicked += WarningOkPressed;
        }

        private void OnDisable()
        {
            newGameButton.clicked -= NewGameButtonPressed;
            loadButton.clicked -= LoadButtonPressed;
            optionButton.clicked -= OptionButtonPressed;
            exitButton.clicked -= ExitButtonPressed;
            newgame_YesButton.clicked -= NewGame_YesPressed;
            newgame_NoButton.clicked -= NewGame_NoPressed;
            exit_YesButton.clicked -= Exit_YesPressed;
            exit_NoButton.clicked -= Exit_NoPressed;
        }
    }
}
