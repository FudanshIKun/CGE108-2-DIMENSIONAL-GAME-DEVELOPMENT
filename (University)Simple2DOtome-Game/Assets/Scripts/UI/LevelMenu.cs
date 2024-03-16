using System;
using Otome.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace Otome.UI
{
    public class LevelMenu : MonoBehaviour, IDataManager
    {
        [Header("Setting")]
        public GameManager.SceneList card01SceneTarget;
        public GameManager.SceneList card02SceneTarget;
        public GameManager.SceneList backSceneTarget;
        private GameManager.SceneList sceneTarget;
        
        UIManagement _ui;

        #region Data System

        public void LoadData(GameData data)
        {
            
        }

        public void SaveData(ref GameData data)
        {
            
        }

        #endregion
        
        #region Main Panel

            VisualElement cardElement;
            Button card01;
            VisualElement card01Front;
            VisualElement card01Back;
            Button card02;
            VisualElement card02Front;
            VisualElement card02Back;
            VisualElement warningGamePanel;
            Button okButton;
            VisualElement enteringGamePanel;
            VisualElement winConditionPanel;
            Label conditionText;
            Button goodBye;
            Button yesButton;
            Button noButton;
            Button backButton;
            

            void Card01Pressed()
            {
                if (DataManager.Instance.CheckCompleatedScene(card01SceneTarget, true))
                {
                    _ui.SwitchPanel(cardElement,warningGamePanel);
                }
                else
                {
                    enteringGamePanel.style.display = DisplayStyle.Flex;
                    sceneTarget = card01SceneTarget;
                }
            }

            void Card02Pressed()
            {
                if (DataManager.Instance.CheckCompleatedScene(card02SceneTarget, true))
                {
                    _ui.SwitchPanel(cardElement,warningGamePanel);
                }
                else
                {
                    enteringGamePanel.style.display = DisplayStyle.Flex;
                    sceneTarget = card02SceneTarget;
                }
            }

            void YesButtonPressed()
            {
                GameManager.Instance.LoadScene(sceneTarget);
                yesButton.pickingMode = PickingMode.Ignore;
                
            }

            void NoButtonPressed()
            {
                enteringGamePanel.style.display = DisplayStyle.None;
            }

            void BackButtonPressed()
            {
                GameManager.Instance.LoadScene(backSceneTarget);
            }

            void OkButtonPressed()
            {
                _ui.SwitchPanel(warningGamePanel,cardElement);
            }

            void GoodByePressed()
            {
                GameManager.Instance.LoadScene(GameManager.SceneList.MainMenu);
            }

        #endregion

        void WinCondition()
        {
            if (DataManager.Instance.CheckCompleatedScene(card01SceneTarget, true) &&
                DataManager.Instance.CheckCompleatedScene(card02SceneTarget, true))
            {
                if (DataManager.Instance.CheckPassedLevel(card01SceneTarget) &&
                    DataManager.Instance.CheckPassedLevel(card02SceneTarget))
                {
                    conditionText.text = "Thanks for playing our otome game \n" +
                                         "You have got all the love from them \n" +
                                         "Congraturation!";
                    _ui.SwitchPanel(cardElement, winConditionPanel);
                }
                else if (DataManager.Instance.CheckPassedLevel(card01SceneTarget) ||
                         DataManager.Instance.CheckPassedLevel(card02SceneTarget))
                {
                    _ui.SwitchPanel(cardElement, winConditionPanel);
                }
                else
                {
                    conditionText.text = "Thanks for playing our otome game \n" +
                                         "You haven't receive any love from them at all \n" +
                                         "You loser!";
                    _ui.SwitchPanel(cardElement, winConditionPanel);
                }
            }
        }

        void PreparedUI()
        {
            enteringGamePanel.style.display = DisplayStyle.None;
            warningGamePanel.style.display = DisplayStyle.None;
            winConditionPanel.style.display = DisplayStyle.None;
        }

        void AddUIFunction()
        {
            card01.clicked += Card01Pressed;
            card02.clicked += Card02Pressed;
            yesButton.clicked += YesButtonPressed;
            noButton.clicked += NoButtonPressed;
            backButton.clicked += BackButtonPressed;
            okButton.clicked += OkButtonPressed;
            goodBye.clicked += GoodByePressed;
        }
        
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _ui = GetComponent<UIManagement>();

            cardElement = root.Q<VisualElement>("CardElement");
            card01 = root.Q<Button>("Card01");
            card02 = root.Q<Button>("Card02");
            card01Front = root.Q<VisualElement>("Card01_Front");
            card01Back = root.Q<VisualElement>("Card01_Back");
            card02Front = root.Q<VisualElement>("Card02_Front");
            card02Back = root.Q<VisualElement>("Card02_Back");
            enteringGamePanel = root.Q<VisualElement>("EnteringGameElement");
            yesButton = root.Q<Button>("YesButton");
            noButton = root.Q<Button>("NoButton");
            backButton = root.Q<Button>("BackButton");
            warningGamePanel = root.Q<VisualElement>("WarningElement");
            okButton = root.Q<Button>("OkButton");
            winConditionPanel = root.Q<VisualElement>("WinConditionElement");
            conditionText = root.Q<Label>("ConditionText");
            goodBye = root.Q<Button>("GoodByeButton");
            
            PreparedUI();
            AddUIFunction();
            WinCondition();
        }

        private void OnDisable()
        {
            card01.clicked -= Card01Pressed;
            card02.clicked -= Card02Pressed;
            yesButton.clicked -= YesButtonPressed;
            noButton.clicked -= NoButtonPressed;
            backButton.clicked -= BackButtonPressed;
        }
    }
}
