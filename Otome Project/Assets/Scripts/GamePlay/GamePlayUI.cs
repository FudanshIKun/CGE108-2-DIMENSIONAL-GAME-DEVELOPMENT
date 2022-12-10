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
        [Range(1f,1.5f)]
        [SerializeField] float bottombarFadeDuration;
        [Range(0.1f, 1.5f)] 
        [SerializeField] float fadeDelay;
        [Range(0.5f, 1.5f)] 
        [SerializeField] float spriteFadeDuration;
        [Range(0.5f, 1.5f)] 
        [SerializeField] float backgroundFadeDuration;
        [Range(0.1f, 1f)]
        [SerializeField] float speakerFadeDuration;
        [Range(0.1f, 1f)] 
        [SerializeField] private float changeSpeakerColorDuration;
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
        
        public IEnumerator SwitchBackground(Texture2D newBG, StoryScene.Sentence.BGTransitionType transitionType)
        {
            float timeElapsed = 0;
            float opacity;
            switch (transitionType)
            {
                case StoryScene.Sentence.BGTransitionType.bg_NormalCharacter_Normal :
                    if (currentBG == bg01)
                    {
                        // Change background02 in the back to new background and;
                        bg02.style.backgroundImage = new StyleBackground(newBG);
                        // make sure background01 is on the front layer;
                        bg01.BringToFront();
                        // make sure that background02 in the back layer is not hidden;
                        bg02.style.opacity = 1;
                        bg02.style.display = DisplayStyle.Flex;
                        // start fading background01's opacity to 0 and hide it;
                        timeElapsed = 0;
                        while (timeElapsed < backgroundFadeDuration)
                        {
                            opacity = Mathf.Lerp(1, 0, timeElapsed / backgroundFadeDuration);
                            timeElapsed += Time.deltaTime;
                            bg01.style.opacity = opacity;
                            yield return null;
                        }
                        bg01.style.display = DisplayStyle.None;
                        // update the state of currentBG;
                        currentBG = bg02;
                    }
                    else if (currentBG == bg02)
                    {
                        // Change background01 in the back to new background and;
                        bg01.style.backgroundImage = new StyleBackground(newBG);
                        // make sure background02 is on the front layer;
                        bg02.BringToFront();
                        // make sure that background01 in the back layer is not hidden;
                        bg01.style.opacity = 1;
                        bg01.style.display = DisplayStyle.Flex;
                        // start fading background02's opacity to 0 and hide it;
                        timeElapsed = 0;
                        while (timeElapsed < backgroundFadeDuration)
                        {
                            opacity = Mathf.Lerp(1, 0, timeElapsed / backgroundFadeDuration);
                            timeElapsed += Time.deltaTime;
                            bg02.style.opacity = opacity;
                            yield return null;
                        }
                        bg02.style.display = DisplayStyle.None;
                        // update the state of currentBG;
                        currentBG = bg01;
                    }
                    break; 
                case StoryScene.Sentence.BGTransitionType.bg_NormalCharacter_Fade :
                    
                    break;
                case StoryScene.Sentence.BGTransitionType.bg_FadeCharacter_Normal :
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
            // check if the bottombar is already showing;
            if (_barState == BottomBarState.Showing)
            {
                yield break;
            }
            // deleate the conversationText.text
            conversationText.text = "";
            // make sure bottombar is visible while the opcaity is still 0;
            bottomBar.style.display = DisplayStyle.Flex;
            // start fading bottombar's opacity to 1;
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
            // make sure that the bottombar has fully showing;
            if (_barState == BottomBarState.Hiding)
            {
                bottomBar.style.opacity = 1;
                _barState = BottomBarState.Showing;
            }
        }
        public IEnumerator FadeHideBottomBar() 
        {
            // check if the bottombar is already hidden;
            if (_barState == BottomBarState.Hiding)
            {
                yield break;
            }
            // start fading bottombar's opacity to 0;
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
            // make sure that the bottombar has fully hidden;
            if (_barState == BottomBarState.Showing)
            {
                bottomBar.style.opacity = 0;
                _barState = BottomBarState.Hiding;
            }
            // make sure bottombar is invisible even the opcaity is 0;
            bottomBar.style.display = DisplayStyle.None;
        }
        
        // change the bottombar speakerName & speakerColor instantly
        public void ChangeSpeaker(string name, Color speakerColor)
        {
            circle.style.unityBackgroundImageTintColor = speakerColor;
            personNameBar.style.unityBackgroundImageTintColor = speakerColor;
            bar.style.borderTopColor = speakerColor;
            bar.style.borderBottomColor = speakerColor;
            bar.style.borderLeftColor = speakerColor;
            bar.style.borderRightColor = speakerColor;
            conversationText.style.color = speakerColor;
            personNameText.text = name;
        }
        public IEnumerator FadeChangeSpeaker(string name, Color speakerColor) // change the bottombar speakerName & speakerColor by fading in time setting
        {
            // get the starter color of the bottombar's speakerColor ( border, circle and personNameBar have the same color )
            Color circleStartColor = new Color(circle.style.unityBackgroundImageTintColor.value.r,
                circle.style.unityBackgroundImageTintColor.value.g,
                circle.style.unityBackgroundImageTintColor.value.b);
            // use this var to dynamic apply color's value that is lerping
            Color dynamicColor;
            // start fading the color from startColor to targetColor;
            float timeElapsed = 0;
            if (personNameText.text != name) // fade the name as well if the name has change;
            {
                personNameText.style.opacity = 0;
                float opacity;
                personNameText.text = name;
                timeElapsed = 0;
                while (timeElapsed < speakerFadeDuration)
                {
                    dynamicColor = Color.Lerp(circleStartColor, speakerColor, timeElapsed / speakerFadeDuration);
                    opacity = Mathf.Lerp(0, 1, timeElapsed / speakerFadeDuration);
                    personNameText.style.opacity = opacity;
                    circle.style.unityBackgroundImageTintColor = dynamicColor;
                    conversationText.style.color = dynamicColor;
                    personNameBar.style.unityBackgroundImageTintColor = dynamicColor;
                    bar.style.borderTopColor = dynamicColor;
                    bar.style.borderBottomColor = dynamicColor;
                    bar.style.borderLeftColor = dynamicColor;
                    bar.style.borderRightColor = dynamicColor;
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                // if the name hasn't change just fade normally;
                timeElapsed = 0;
                while (timeElapsed < speakerFadeDuration)
                {
                    dynamicColor = Color.Lerp(circleStartColor, speakerColor, timeElapsed / speakerFadeDuration);
                    circle.style.unityBackgroundImageTintColor = dynamicColor;
                    conversationText.style.color = dynamicColor;
                    personNameBar.style.unityBackgroundImageTintColor = dynamicColor;
                    bar.style.borderTopColor = dynamicColor;
                    bar.style.borderBottomColor = dynamicColor;
                    bar.style.borderLeftColor = dynamicColor;
                    bar.style.borderRightColor = dynamicColor;
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
            }
            
        }

        public IEnumerator TypeText(string text)
        {
            // wait a second before start typing;
            yield return new WaitForSeconds(0.5f);
            // delete the old conversationText;
            conversationText.text = "";
            // Update the typing state
            typingState = TypingState.Playing;
            // start typing
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
        
        public IEnumerator AskQuestion()
        {
            yield return null;
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
        
        public IEnumerator ChangeSprite(StoryScene.Sentence.EmotionList emotion,
            Character character, 
            StoryScene.Sentence.CharacterTransitionType transitionType)
        {
            // Sprite TransitionType set in the StoryScene.Sentence
            switch (transitionType)
            {
                // InstantlyChangeSprite & Emotion;
                case StoryScene.Sentence.CharacterTransitionType.Normal :
                    switch (emotion)
                    {
                        case StoryScene.Sentence.EmotionList.normal :
                            characterSprite.style.backgroundImage = new StyleBackground(character.normal);
                            break;
                        case StoryScene.Sentence.EmotionList.smile :
                            characterSprite.style.backgroundImage = new StyleBackground(character.smile);
                            break;
                        case StoryScene.Sentence.EmotionList.angry :
                            characterSprite.style.backgroundImage = new StyleBackground(character.angry);
                            break;
                    }
                    break;
                // Fade Change Character Sprite to hide then change it and fade it on;
                case StoryScene.Sentence.CharacterTransitionType.Fade :
                    // Deleate Old Text & Name;
                    personNameText.text = "";
                    conversationText.text = "";
                    // Start Fading Hide BottomBar
                    float timeElapsed = 0;
                    float opacity;
                    while (timeElapsed < changeSpeakerColorDuration)
                    {
                        opacity = Mathf.Lerp(1, 0, timeElapsed / changeSpeakerColorDuration);
                        timeElapsed += Time.deltaTime;
                        bar.style.opacity = opacity;
                        circle.style.opacity = opacity;
                        personNameBar.style.opacity = opacity;
                        yield return null;
                    }
                    circle.style.display = DisplayStyle.None;
                    personNameBar.style.display = DisplayStyle.None;
                    // Fade Hide Sprite;
                    yield return StartCoroutine(FadeHideSprite());
                    // Change Sprite Emotion;
                    switch (emotion)
                    {
                        case StoryScene.Sentence.EmotionList.normal :
                            characterSprite.style.backgroundImage = new StyleBackground(character.normal);
                            break;
                        case StoryScene.Sentence.EmotionList.smile :
                            characterSprite.style.backgroundImage = new StyleBackground(character.smile);
                            break;
                        case StoryScene.Sentence.EmotionList.angry :
                            characterSprite.style.backgroundImage = new StyleBackground(character.angry);
                            break;
                    }
                    // Fade Show Sprite;
                    yield return StartCoroutine( FadeShowSprite());
                    circle.style.display = DisplayStyle.Flex;
                    personNameBar.style.display = DisplayStyle.Flex;
                    // Start Fading Show BottomBar
                    timeElapsed = 0;
                    while (timeElapsed < changeSpeakerColorDuration)
                    {
                        opacity = Mathf.Lerp(0, 1, timeElapsed / changeSpeakerColorDuration);
                        bar.style.opacity = opacity;
                        circle.style.opacity = opacity;
                        personNameBar.style.opacity = opacity;
                        timeElapsed += Time.deltaTime;
                        yield return null;
                    }
                    yield return new WaitForSeconds(1f);
                    break;
            }

        }

        public IEnumerator FadeShowSprite()
        {
            // check if the sprite are already in show state;
            if (spriteState == SpriteState.Showing)
            {
                yield break;
            }
            
            // fade the sprite opacity to 1;
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
            
            // check if the sprite hasn't compleate show up yet then make it compleately show;
            if (spriteState == SpriteState.Hiding)
            {
                characterSprite.style.opacity = 0;
                spriteState = SpriteState.Showing;
            }
        }

        public IEnumerator FadeHideSprite()
        {
            // check if the sprite are already in hide state;
            if (spriteState == SpriteState.Hiding)
            {
                yield break;
            }
            
            // fade the sprite opacity to 0;
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
            
            // check if the sprite hasn't compleate dissapear yet then make it compleately gone;
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
    }
}
