using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Otome.GamePlay
{
    public class GamePlayUI : MonoBehaviour
    {
        [Header("UI Setting")] 
        private GamePlayManager _gamePlayManager;
        [Range(1f,2f)]
        [SerializeField] float fadeDuration;
        [Range(1f, 2f)] 
        [SerializeField] float fadeDelay;
        private BottomBarState _barState = BottomBarState.hiding;
        [HideInInspector]
        public TypingState typingState = TypingState.Compleated;
        
        public VisualElement root;
        
        #region UI Management

        Button pauseButton;        
        
        #endregion
                
        #region Background Management
        
        private int currentBG = 1;
        private VisualElement bg01;
        private VisualElement bg02;
        
        public void ChangeBackground(int numberOfBG ) // *** Using in GamePlayManager ***
        {
            switch (numberOfBG)
            {
                case 1 :
                    Debug.Log("Change Background number 0");
                    break;
                case 2 :
                    Debug.Log("Change Background number 1");
                    break;
                default :
                    break;
            }
                    
        }
        public void SwitchBackground() // *** Using in GamePlayManager ***
        {
            switch (currentBG)
            {
                case 1 : 
                    Debug.Log("Set BG01's oppacity to 0");
                    currentBG = 2; 
                    break;
                case 2 :
                    Debug.Log("Set BG01's oppacity to 100");
                    currentBG = 1;
                    break;
                default :
                    break;
            }
        }
        
        #endregion
        
        #region BottomBar Management
        
        public enum TypingState
        { 
            Playing,
            Compleated
        }

        VisualElement bottomBar;
        VisualElement bar;
        VisualElement circle;
        Label conversationText;
        VisualElement personNameBar;
        Label personNameText;
        List<VisualElement> BottomBarElement;

        public enum BottomBarState
        {
            Showing,
            hiding
        }

        public IEnumerator FadeShowBottomBar() 
        {
            if (_barState == BottomBarState.Showing)
            {
                yield break;
            }

            conversationText.text = "";
            bottomBar.style.display = DisplayStyle.Flex;
            float timeElapsed = 0;
            float opacity;
            while (timeElapsed < fadeDuration)
            {
                opacity = Mathf.Lerp(0, 1, timeElapsed / fadeDuration);
                timeElapsed += Time.deltaTime;
                bottomBar.style.opacity = opacity;
                if (bottomBar.style.opacity == 1) { _barState = BottomBarState.Showing; }
                yield return null;
            }
            if (_barState == BottomBarState.hiding)
            {
                bottomBar.style.opacity = 1;
                _barState = BottomBarState.Showing;
            }
        }
        public IEnumerator FadeHideBottomBar() 
        {
            yield return new WaitForSeconds(fadeDelay);
            if (_barState == BottomBarState.hiding)
            {
                yield break;
            }
            float timeElapsed = 0;
            float opacity;
            while (timeElapsed < fadeDuration)
            {
                opacity = Mathf.Lerp(1, 0, timeElapsed / fadeDuration);
                timeElapsed += Time.deltaTime;
                bottomBar.style.opacity = opacity;
                if (bottomBar.style.opacity == 0) { _barState = BottomBarState.hiding; }
                yield return null;
            }
            if (_barState == BottomBarState.Showing)
            {
                bottomBar.style.opacity = 0;
                _barState = BottomBarState.hiding;
            }
            bottomBar.style.display = DisplayStyle.None;
        }
        public void ChangeSpeaker(string name, Color speakerColor) // *** Using in GamePlayManager ***
        {
            personNameText.text = name;
            personNameBar.style.unityBackgroundImageTintColor = speakerColor;
            conversationText.style.color = speakerColor;
            circle.style.unityBackgroundImageTintColor = speakerColor;
        }

        public IEnumerator TypeText(string text)
        {
            conversationText.text = "";
            typingState = TypingState.Playing;
            int wordIndex = 0;
            while (typingState != TypingState.Compleated)
            {
                conversationText.text += text[wordIndex];
                yield return new WaitForSeconds(0.08f);
                if (++wordIndex == text.Length)
                {
                    typingState = TypingState.Compleated;
                    break;
                }
            }
        }
        
        #endregion
        
        #region Character Management
        
        private VisualElement CharacterSprite;
        
        public void SetSprite(Sprite targetSprite) // *** Using in GamePlayManager ***
        { 
            CharacterSprite.style.backgroundImage = new StyleBackground(targetSprite);
        }
        
        #endregion
        
        void PausedPressed()
        {
            
        }

        void PreparedUI()
        {
            _barState = BottomBarState.hiding;
            bottomBar.style.opacity = 0;
            bottomBar.style.display = DisplayStyle.None;
            typingState = TypingState.Compleated;
            
        }
        void AddFunctionUI()
        {
            _gamePlayManager = FindObjectOfType<GamePlayManager>();
            pauseButton.clicked += PausedPressed;
            bg01.AddManipulator(new Clickable(evt => StartCoroutine(_gamePlayManager.PlayNextSentence())));
            bg02.AddManipulator(new Clickable(evt => StartCoroutine(_gamePlayManager.PlayNextSentence())));
            bottomBar.AddManipulator(new Clickable(evt => StartCoroutine(_gamePlayManager.PlayNextSentence())));
        }
        
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            bg01 = root.Q<VisualElement>("Background01");
            bg02 = root.Q<VisualElement>("Background02");
            bottomBar = root.Q<VisualElement>("BottomBar");
            bar = root.Q<VisualElement>("Bar");
            circle = root.Q<VisualElement>("Circle");
            personNameBar = root.Q<VisualElement>("NameBar");
            personNameText = root.Q<Label>("PersonNameText");
            conversationText = root.Q<Label>("ConversationText");
            pauseButton = root.Q<Button>("PauseButton");
            CharacterSprite = root.Q<VisualElement>("Character");
            
            PreparedUI();
            AddFunctionUI();
        }

        private void OnValidate()
        {
            
        }
    }
}
