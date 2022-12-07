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
        [SerializeField] float bottombarFadeDuration;
        [Range(1f, 2f)] 
        [SerializeField] float fadeDelay;
        [Range(1f, 2f)] 
        [SerializeField] float spriteFadeDuration;
        [Range(1f, 2f)] 
        [SerializeField] float backgroundFadeDuration;
        private BottomBarState _barState = BottomBarState.Hiding;
        [HideInInspector]
        public TypingState typingState = TypingState.Compleated;

        public VisualElement root;
        
        #region UI Management

        Button diaryButton;
        Button settingButton;
        VisualElement fadePanel;

        void DiaryPressed()
        {
            
        }

        void SettingPressed()
        {
            
        }
        
        void PausedPressed()
        {
            _gamePlayManager.isPaused = true;
        }

        void UnPaused()
        {
            _gamePlayManager.isPaused = false;
        }
        
        #endregion
                
        #region Background Management

        GroupBox bgGroup;
        VisualElement bg01;
        VisualElement bg02;

        VisualElement currentBG;
        
        public IEnumerator SwitchBackgroundWithoutFade(Texture2D newBG) // *** Using in GamePlayManager ***
        {
            float timeElapsed = 0;
            float opacity;
            switch (currentBG)
            {
                case var value when value == bg01 :
                    Debug.Log("Change currentBG to bg02");
                    bg02.style.backgroundImage = new StyleBackground(newBG);
                    bg01.BringToFront();
                    bg02.style.opacity = 1;
                    bg02.style.display = DisplayStyle.Flex;
                    timeElapsed = 0;
                    while (timeElapsed < backgroundFadeDuration)
                    {
                        opacity = Mathf.Lerp(1, 0, timeElapsed / backgroundFadeDuration);
                        timeElapsed += Time.deltaTime;
                        bg01.style.opacity = opacity;
                        yield return null;
                    }
                    bg01.style.display = DisplayStyle.None;
                    currentBG = bg02;
                    break;
                case var value when value == bg02 :
                    Debug.Log("Change currentBG to bg01");
                    bg01.style.backgroundImage = new StyleBackground(newBG);
                    bg02.BringToFront();
                    bg01.style.opacity = 1;
                    bg01.style.display = DisplayStyle.Flex;
                    timeElapsed = 0;
                    while (timeElapsed < backgroundFadeDuration)
                    {
                        opacity = Mathf.Lerp(1, 0, timeElapsed / backgroundFadeDuration);
                        timeElapsed += Time.deltaTime;
                        bg02.style.opacity = opacity;
                        yield return null;
                    }
                    bg02.style.display = DisplayStyle.None;
                    currentBG = bg01;
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
            Hiding
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
            while (timeElapsed < bottombarFadeDuration)
            {
                opacity = Mathf.Lerp(0, 1, timeElapsed / bottombarFadeDuration);
                timeElapsed += Time.deltaTime;
                bottomBar.style.opacity = opacity;
                if (bottomBar.style.opacity == 1) { _barState = BottomBarState.Showing; }
                yield return null;
            }
            if (_barState == BottomBarState.Hiding)
            {
                bottomBar.style.opacity = 1;
                _barState = BottomBarState.Showing;
            }
        }
        public IEnumerator FadeHideBottomBar() 
        {
            yield return new WaitForSeconds(fadeDelay);
            if (_barState == BottomBarState.Hiding)
            {
                yield break;
            }
            float timeElapsed = 0;
            float opacity;
            while (timeElapsed < bottombarFadeDuration)
            {
                opacity = Mathf.Lerp(1, 0, timeElapsed / bottombarFadeDuration);
                timeElapsed += Time.deltaTime;
                bottomBar.style.opacity = opacity;
                if (bottomBar.style.opacity == 0) { _barState = BottomBarState.Hiding; }
                yield return null;
            }
            if (_barState == BottomBarState.Showing)
            {
                bottomBar.style.opacity = 0;
                _barState = BottomBarState.Hiding;
            }
            bottomBar.style.display = DisplayStyle.None;
        }

        public void ChangeSpeaker(string name, Color speakerColor) // *** Using in GamePlayManager ***
        {
            personNameText.text = name;
            personNameBar.style.unityBackgroundImageTintColor = speakerColor;
            conversationText.style.color = speakerColor;
            bar.style.borderTopColor = speakerColor;
            bar.style.borderBottomColor = speakerColor;
            bar.style.borderLeftColor = speakerColor;
            bar.style.borderRightColor = speakerColor;
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
        
        private VisualElement characterSprite;

        public enum SpriteState
        {
            Hiding,
            Showing
        }
        [HideInInspector]
        public SpriteState spriteState = SpriteState.Hiding;
        
        public void ChangeSprite(Sprite targetSprite) // *** Using in GamePlayManager ***
        {
            characterSprite.style.backgroundImage = new StyleBackground(targetSprite);
        }

        public IEnumerator FadeShowSprite()
        {
            if (spriteState == SpriteState.Showing)
            {
                yield break;
            }

            characterSprite.style.display = DisplayStyle.Flex;
            float timeElapsed = 0;
            float opacity;
            while (timeElapsed < spriteFadeDuration)
            {
                opacity = Mathf.Lerp(0, 1, timeElapsed / spriteFadeDuration);
                timeElapsed += Time.deltaTime;
                characterSprite.style.opacity = opacity;
                if (characterSprite.style.opacity == 0) { spriteState = SpriteState.Showing; }
                yield return null;
            }
            if (spriteState == SpriteState.Hiding)
            {
                characterSprite.style.opacity = 0;
                spriteState = SpriteState.Showing;
            }
        }

        public IEnumerator FadeHideSprite()
        {
            yield return new WaitForSeconds(fadeDelay);
            if (spriteState == SpriteState.Showing)
            {
                yield break;
            }
            float timeElapsed = 0;
            float opacity;
            while (timeElapsed < spriteFadeDuration)
            {
                opacity = Mathf.Lerp(1, 0, timeElapsed / spriteFadeDuration);
                timeElapsed += Time.deltaTime;
                characterSprite.style.opacity = opacity;
                if (characterSprite.style.opacity == 0) { spriteState = SpriteState.Hiding; }
                yield return null;
            }
            if (spriteState == SpriteState.Showing)
            {
                characterSprite.style.opacity = 0;
                spriteState = SpriteState.Hiding;
            }
            characterSprite.style.display = DisplayStyle.None;
        }

        #endregion

        void PreparedUI()
        {
            _barState = BottomBarState.Hiding;
            spriteState = SpriteState.Hiding;
            bottomBar.style.opacity = 0;
            bottomBar.style.display = DisplayStyle.None;
            characterSprite.style.opacity = 0;
            characterSprite.style.display = DisplayStyle.None;
            typingState = TypingState.Compleated;
            currentBG = bg01;
            fadePanel.style.opacity = 0;
            fadePanel.style.display = DisplayStyle.None;

        }
        void AddFunctionUI()
        {
            _gamePlayManager = FindObjectOfType<GamePlayManager>();
            diaryButton.clicked += DiaryPressed;
            diaryButton.clicked += PausedPressed;
            settingButton.clicked += SettingPressed;
            settingButton.clicked += PausedPressed;
            bg01.AddManipulator(new Clickable(evt => StartCoroutine(_gamePlayManager.PlayNextSentence())));
            bg02.AddManipulator(new Clickable(evt => StartCoroutine(_gamePlayManager.PlayNextSentence())));
            bottomBar.AddManipulator(new Clickable(evt => StartCoroutine(_gamePlayManager.PlayNextSentence())));
            fadePanel.AddManipulator(new Clickable(evt => StartCoroutine(_gamePlayManager.PlayNextSentence())));
        }
        
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            bgGroup = root.Q<GroupBox>("Backgrounds");
            bg01 = root.Q<VisualElement>("Background01");
            bg02 = root.Q<VisualElement>("Background02");
            bottomBar = root.Q<VisualElement>("BottomBar");
            bar = root.Q<VisualElement>("Bar");
            circle = root.Q<VisualElement>("Circle");
            personNameBar = root.Q<VisualElement>("NameBar");
            personNameText = root.Q<Label>("PersonNameText");
            conversationText = root.Q<Label>("ConversationText");
            diaryButton = root.Q<Button>("DiaryButton");
            settingButton = root.Q<Button>("SettingButton");
            characterSprite = root.Q<VisualElement>("Character");
            fadePanel = root.Q<VisualElement>("FadePanel");
            
            PreparedUI();
            AddFunctionUI();
        }

        void Update()
        {
            
        }

        private void OnValidate()
        {
            
        }
    }
}
