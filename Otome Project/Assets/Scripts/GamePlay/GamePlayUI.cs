using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Otome.Core;

namespace Otome.GamePlay
{
    public class GamePlayUI : MonoBehaviour
    {
        #region Setting

            [Header("Setting")]
        
            private GamePlayManager _gamePlayManager;
        
            [Range(1f,1.5f)] [SerializeField] float bottombarFadeDuration;
        
            [Range(0.5f, 1.5f)] [SerializeField] float spriteFadeDuration;
        
            [Range(0.5f, 1.5f)] [SerializeField] float backgroundFadeDuration;
        
            [Range(0.1f, 1f)] [SerializeField] float speakerFadeDuration;
        
            [Range(0.1f, 1f)] [SerializeField] private float changeSpeakerColorDuration;
        
            private BottomBarState bottombarState = BottomBarState.Hiding;
        
            [HideInInspector] public TypingState typingState = TypingState.Compleated;

        #endregion

        #region UI Management
        
            Button settingButton;
            VisualElement fadePanel;
            GroupBox questionPanel;
            GroupBox endGamePanel;
            VisualElement sceneLoader;
            Button answerButton01;
            Button answerButton02;
            Button answerButton03;
            Label _answerText01;
            Label _answerText02;
            Label _answerText03;

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
        
            VisualElement bg01;
            VisualElement bg02;

            VisualElement currentBG;

            public void SetBackground(Texture2D newBG, int numberOfBg)
            {
                if (numberOfBg == 1)
                {
                    bg01.style.backgroundImage = new StyleBackground(newBG);
                }
                else if (numberOfBg == 2)
                {
                    bg02.style.backgroundImage = new StyleBackground(newBG);
                }
            }
        
            public IEnumerator SwitchBackground(Texture2D newBG, 
                StoryScene.Sentence.BGTransitionType transitionType)
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
                            currentBG = bg02; // update the state of currentBG;
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
                            currentBG = bg01; // update the state of currentBG;
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
                if (bottombarState == BottomBarState.Showing)
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
                    if (bottomBar.style.opacity == 1) { bottombarState = BottomBarState.Showing; }
                    yield return null;
                }
                
                // make sure that the bottombar has fully showing;
                if (bottombarState == BottomBarState.Hiding)
                {
                    bottomBar.style.opacity = 1;
                    bottombarState = BottomBarState.Showing;
                }
            }
            public IEnumerator FadeHideBottomBar() 
            {
                // check if the bottombar is already hidden;
                if (bottombarState == BottomBarState.Hiding)
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
                    if (bottomBar.style.opacity == 0) { bottombarState = BottomBarState.Hiding; }
                    yield return null;
                }
                
                // make sure that the bottombar has fully hidden;
                if (bottombarState == BottomBarState.Showing)
                {
                    bottomBar.style.opacity = 0;
                    bottombarState = BottomBarState.Hiding;
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
            
            // change the bottombar speakerName & speakerColor by fading in time setting
            public IEnumerator FadeChangeSpeaker(string name, Color speakerColor) 
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
                        dynamicColor = Color.Lerp(circleStartColor, speakerColor, 
                            timeElapsed / speakerFadeDuration);
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
                        dynamicColor = Color.Lerp(circleStartColor, speakerColor, 
                            timeElapsed / speakerFadeDuration);
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
        
            // ReSharper disable Unity.PerformanceAnalysis
            public IEnumerator AskQuestion(List<StoryScene.ChoiceLabel> Choices)
            {
                if (Choices.Count > 3)
                {
                    Debug.Log("Detected more than 3 choice in a question");
                    yield break;
                }

                _answerText01.text = Choices[0].text;
                _answerText02.text = Choices[1].text;
                _answerText03.text = Choices[2].text;
            
                // FadeShow Question
                float timeElapsed = 0;
                float opacity;
                questionPanel.style.display = DisplayStyle.Flex;
                while (timeElapsed < bottombarFadeDuration)
                {
                    opacity = Mathf.Lerp(0, 1, timeElapsed / bottombarFadeDuration);
                    questionPanel.style.opacity = opacity;
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                yield return new WaitForSeconds(_gamePlayManager.NextConversationDelay);
                answerButton01.pickingMode = PickingMode.Position;
                answerButton02.pickingMode = PickingMode.Position;
                answerButton03.pickingMode = PickingMode.Position;
            }

            public IEnumerator Answered()
            {
                // FadeHide Question
                float timeElapsed = 0;
                float opacity;
                questionPanel.style.display = DisplayStyle.Flex;
                while (timeElapsed < bottombarFadeDuration)
                {
                    opacity = Mathf.Lerp(1, 0, timeElapsed / bottombarFadeDuration);
                    questionPanel.style.opacity = opacity;
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                questionPanel.style.display = DisplayStyle.None;
            }

            void Answer01Clicked()
            {
                // UI Process
                StartCoroutine(Answered());
                _gamePlayManager.choiceChosen = 0;
                _gamePlayManager.SwitchScene();
                _gamePlayManager.currentQuestionHasAnswered = true;
                _gamePlayManager.conversationState = GamePlayManager.ConversationState.COMPLEATED;
                answerButton01.pickingMode = PickingMode.Ignore;
                answerButton02.pickingMode = PickingMode.Ignore;
                answerButton03.pickingMode = PickingMode.Ignore;
                
                // GamePlay Process
                _gamePlayManager.CheckCollectChoice();

                // Story Process
                StartCoroutine(_gamePlayManager.PlayNextSentence());
            }
            void Answer02Clicked()
            {
                // UI Process
                StartCoroutine(Answered());
                _gamePlayManager.choiceChosen = 1;
                _gamePlayManager.SwitchScene();
                _gamePlayManager.currentQuestionHasAnswered = true;
                _gamePlayManager.conversationState = GamePlayManager.ConversationState.COMPLEATED;
                answerButton01.pickingMode = PickingMode.Ignore;
                answerButton02.pickingMode = PickingMode.Ignore;
                answerButton03.pickingMode = PickingMode.Ignore;
                
                // GamePlay Process
                _gamePlayManager.CheckCollectChoice();

                // Story Process
                StartCoroutine(_gamePlayManager.PlayNextSentence());
            }
            void Answer03Clicked()
            {
                // UI Process
                StartCoroutine(Answered());
                _gamePlayManager.choiceChosen = 2;
                _gamePlayManager.SwitchScene();
                _gamePlayManager.currentQuestionHasAnswered = true;
                _gamePlayManager.conversationState = GamePlayManager.ConversationState.COMPLEATED;
                answerButton01.pickingMode = PickingMode.Ignore;
                answerButton02.pickingMode = PickingMode.Ignore;
                answerButton03.pickingMode = PickingMode.Ignore;
                
                // GamePlay Process
                _gamePlayManager.CheckCollectChoice();

                // Story Process
                StartCoroutine(_gamePlayManager.PlayNextSentence());
            }
        
        
        #endregion
        
        #region Sprite Management
        
            private VisualElement characterSprite;

            public enum SpriteState
            {
                Hiding,
                Showing
            }
            [HideInInspector] public SpriteState spriteState = SpriteState.Hiding;
        
            public void ChangeSprite(StoryScene.Sentence.EmotionList emotion, Character character)
            {
                // Sprite TransitionType set in the StoryScene.Sentence
                switch (emotion)
                {
                    // InstantlyChangeSprite & Emotion;
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
            bottombarState = BottomBarState.Hiding;
            spriteState = SpriteState.Hiding;
            bottomBar.style.opacity = 0;
            bottomBar.style.display = DisplayStyle.None;
            characterSprite.style.opacity = 0;
            characterSprite.style.display = DisplayStyle.None;
            typingState = TypingState.Compleated;
            currentBG = bg01;
            fadePanel.style.opacity = 0;
            fadePanel.style.display = DisplayStyle.None;
            questionPanel.style.display = DisplayStyle.None;
            questionPanel.style.opacity = 0;
            answerButton01.pickingMode = PickingMode.Ignore;
            answerButton02.pickingMode = PickingMode.Ignore;
            answerButton03.pickingMode = PickingMode.Ignore;

        }
        void AddFunctionUI()
        {
            _gamePlayManager = FindObjectOfType<GamePlayManager>();
            settingButton.clicked += SettingPressed;
            settingButton.clicked += PausedPressed;
            bg01.AddManipulator(new Clickable(evt => 
                StartCoroutine(_gamePlayManager.PlayNextSentence())));
            bg02.AddManipulator(new Clickable(evt => 
                StartCoroutine(_gamePlayManager.PlayNextSentence())));
            bottomBar.AddManipulator(new Clickable(evt => 
                StartCoroutine(_gamePlayManager.PlayNextSentence())));
            fadePanel.AddManipulator(new Clickable(evt => 
                StartCoroutine(_gamePlayManager.PlayNextSentence())));
            answerButton01.clicked += Answer01Clicked;
            answerButton02.clicked += Answer02Clicked;
            answerButton03.clicked += Answer03Clicked;
            endGamePanel.style.display = DisplayStyle.None;
            endGamePanel.style.opacity = 0;
            sceneLoader.style.display = DisplayStyle.Flex;
            sceneLoader.style.opacity = 1;
        }
        
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            // Get Elements
            bg01 = root.Q<VisualElement>("Background01");
            bg02 = root.Q<VisualElement>("Background02");
            bottomBar = root.Q<VisualElement>("BottomBar");
            bar = root.Q<VisualElement>("Bar");
            circle = root.Q<VisualElement>("Circle");
            personNameBar = root.Q<VisualElement>("NameBar");
            personNameText = root.Q<Label>("PersonNameText");
            conversationText = root.Q<Label>("ConversationText");
            settingButton = root.Q<Button>("SettingButton");
            characterSprite = root.Q<VisualElement>("Character");
            fadePanel = root.Q<VisualElement>("FadePanel");
            questionPanel = root.Q<GroupBox>("QuestionPanel");
            answerButton01 = root.Q<Button>("Answer01");
            answerButton02 = root.Q<Button>("Answer02");
            answerButton03 = root.Q<Button>("Answer03");
            _answerText01 = root.Q<Label>("Answer01Text");
            _answerText02 = root.Q<Label>("Answer02Text");
            _answerText03 = root.Q<Label>("Answer03Text");
            endGamePanel = root.Q<GroupBox>("EndGamePanel");
            sceneLoader = root.Q<VisualElement>("SceneLoader");
            
            
            PreparedUI();
            AddFunctionUI();
        }
    }
}
