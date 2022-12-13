using System;
using System.Collections;
using UnityEngine;

namespace Otome.GamePlay
{
    public class GamePlayManager : MonoBehaviour
    {
        #region Singleton

        public static GamePlayManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Detect more than one GamePlayManager!");
            }

            Instance = this;
        }

        #endregion

        [Header("Story Setting")] 
        [Range(0f, 1f)] 
        [HideInInspector] public float NextConversationDelay;
        [Space(5)]
        [Header("Audio Setting")]
        public AudioSource bgSpeaker;
        public AudioSource sfxSpeaker;
        [Space(5)]
        [Header("GamePlay Setting")]
        public GamePlayState gameState = GamePlayState.PLAYING;

        [HideInInspector] public bool currentQuestionHasAnswered;
        [HideInInspector] public bool isPaused;
        
        #region UI management

        private GamePlayUI mainUI;
        
        #endregion

        #region Story Management

        [SerializeField] StoryScene currentSceneStory;
        private StoryScene nextSceneStory;
        [HideInInspector] public int sentenceIndex = -1;
        [HideInInspector] public int questionIndex = 0;
        public enum ConversationState
        {
            PLAYING, COMPLEATED
        }
        [HideInInspector] public ConversationState conversationState = ConversationState.COMPLEATED;
        private Texture2D currenntPlace;
        [HideInInspector] public int choiceChosen;

        // ReSharper disable Unity.PerformanceAnalysis
        public IEnumerator PlayNextSentence()
        {
            Debug.Log("Player has call sentence");
            
            if (nextSceneStory != null)
            {
                Debug.Log("Change Story Path");
                currentSceneStory = nextSceneStory;
                sentenceIndex = -1;
                questionIndex = 0;
                nextSceneStory = null;
            }
            
            // Check if the conversation hasn't end but player call this function;
            if (conversationState == ConversationState.PLAYING)
            {
                Debug.Log("The sentence's still playing");
                yield break;
            }

            // if the conversation has COMPLEATE set it to PLAYING when player call this function again;
            conversationState = ConversationState.PLAYING;
            
            // Check if there is no next sentence in the index then break;
            if (sentenceIndex + 1 >= currentSceneStory.Sentences.Count)
            {
                Debug.Log("There are no more sentence");
                yield break;
            }
            
            if (currentSceneStory.Sentences[questionIndex].sentenceType == StoryScene.Sentence.SentenceType.Question )
            {
                if (currentQuestionHasAnswered == false)
                {
                    Debug.Log("Player hasn't answer the question yet");
                    yield break;
                }
            }
            
            // Check if the sentence set in new location location
            if (currentSceneStory.Sentences[++sentenceIndex].Place != currenntPlace)
            {
                //Debug.Log("Start Sentence of " + sentenceIndex);
                //Debug.Log(sentenceIndex + " has the new location");
                
                // Reset Current Question Bool;
                currentQuestionHasAnswered = false;
                
                // If character TranstionType is Fade
                if (currentSceneStory.Sentences[sentenceIndex].characterTransitionType ==
                    StoryScene.Sentence.CharacterTransitionType.Fade)
                {
                    //Debug.Log(sentenceIndex + " has characterTransition Type of Fade");
                    
                    // FadeHide Sprite
                    StartCoroutine(mainUI.FadeHideSprite());
                    
                    //FadeHide BottomBar
                    yield return StartCoroutine(mainUI.FadeHideBottomBar());
                
                    // Switch Background if the new sentence has new location;
                    yield return StartCoroutine(mainUI.SwitchBackground(
                        currentSceneStory.Sentences[sentenceIndex].Place, 
                        currentSceneStory.Sentences[sentenceIndex].bgTransitionType));
                
                    // Change character sprite by the emotion setting in sentence;
                    mainUI.ChangeSprite(currentSceneStory.Sentences[sentenceIndex].SpriteEmotion,
                        currentSceneStory.Sentences[sentenceIndex].character);
                
                    // FadeShow Sprite
                    yield return StartCoroutine(mainUI.FadeShowSprite());
                    
                    // FadeShow BottomBar
                    StartCoroutine(mainUI.FadeShowBottomBar());
                
                    // update the currentPlace for check if there's new location;
                    currenntPlace = currentSceneStory.Sentences[sentenceIndex].Place;
                
                    // FadeChange Bottombar Color if the new sentence has the new character
                    yield return StartCoroutine( mainUI.FadeChangeSpeaker(
                        currentSceneStory.Sentences[sentenceIndex].character.characterName, 
                        currentSceneStory.Sentences[sentenceIndex].character.characterColor));

                    // Check if conversationType is Question;
                    if (currentSceneStory.Sentences[sentenceIndex].sentenceType ==
                        StoryScene.Sentence.SentenceType.Question)
                    {
                        //Debug.Log(sentenceIndex + " has sentence Type of Question");
                        
                        // Start typing conversation;
                        yield return StartCoroutine(
                        mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                        
                        // Start asking question;
                        yield return StartCoroutine(mainUI.AskQuestion(
                            currentSceneStory.Sentences[sentenceIndex].questionChoices));
                        
                        questionIndex = sentenceIndex;
                    }
                    
                    // Check if conversationType is Normal
                    else if (currentSceneStory.Sentences[sentenceIndex].sentenceType == 
                         StoryScene.Sentence.SentenceType.NormalConversation)
                    {
                        //Debug.Log(sentenceIndex + " has sentence Type of Normal");
                        
                        // Start typing conversation;
                        yield return StartCoroutine(
                        mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                        
                        questionIndex = sentenceIndex;
                    }
                }
                
                // If character TranstionType is Normal
                else if (currentSceneStory.Sentences[sentenceIndex].characterTransitionType ==
                          StoryScene.Sentence.CharacterTransitionType.Normal)
                {
                    //Debug.Log(sentenceIndex + " has characterTransition Type of Normal");
                    
                    // Switch Background if the new sentence has new location;
                    yield return StartCoroutine(mainUI.SwitchBackground(
                        currentSceneStory.Sentences[sentenceIndex].Place, 
                        currentSceneStory.Sentences[sentenceIndex].bgTransitionType));
                    
                    // Change component's color of the BottomBar;
                    mainUI.FadeChangeSpeaker(currentSceneStory.Sentences[sentenceIndex].character.characterName, 
                        currentSceneStory.Sentences[sentenceIndex].character.characterColor);
                    
                    // Change Sprite Emotion;
                    mainUI.ChangeSprite(currentSceneStory.Sentences[sentenceIndex].SpriteEmotion,
                        currentSceneStory.Sentences[sentenceIndex].character);
                    
                    // Check if conversationType is Question;
                    if (currentSceneStory.Sentences[sentenceIndex].sentenceType ==
                        StoryScene.Sentence.SentenceType.Question)
                    {
                        //Debug.Log(sentenceIndex + " has sentence Type of Question");
                        
                        // Start typing conversation;
                        yield return StartCoroutine(
                            mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                        
                        // Start asking question;
                        yield return StartCoroutine(mainUI.AskQuestion(
                            currentSceneStory.Sentences[sentenceIndex].questionChoices));
                        
                        questionIndex = sentenceIndex;
                    }
                    
                    // Check if conversationType is Normal
                    else if (currentSceneStory.Sentences[sentenceIndex].sentenceType == 
                             StoryScene.Sentence.SentenceType.NormalConversation)
                    {
                        //Debug.Log(sentenceIndex + " has sentence Type of Normal");
                        
                        // Start typing conversation;
                        yield return StartCoroutine(
                            mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                        
                        questionIndex = sentenceIndex;
                    }
                }
            }
            
            // if the new sentence has the same location as the previous one
            else if (currentSceneStory.Sentences[sentenceIndex].Place == currenntPlace)
            {
                //Debug.Log("Start Sentence of " + sentenceIndex);
                //Debug.Log(sentenceIndex + " has the old location");
                
                // Reset Current Question Bool;
                currentQuestionHasAnswered = false;
                
                // If character TranstionType is Fade
                if (currentSceneStory.Sentences[sentenceIndex].characterTransitionType ==
                    StoryScene.Sentence.CharacterTransitionType.Fade)
                {
                    //Debug.Log(sentenceIndex + " has characterTransition Type of Fade");
                    
                    // FadeHide Sprite
                    StartCoroutine(mainUI.FadeHideSprite());
                    
                    //FadeHide BottomBar
                    yield return StartCoroutine(mainUI.FadeHideBottomBar());

                    // Change character sprite by the emotion setting in sentence;
                    mainUI.ChangeSprite(currentSceneStory.Sentences[sentenceIndex].SpriteEmotion,
                        currentSceneStory.Sentences[sentenceIndex].character);
                
                    // FadeShow Sprite
                    yield return StartCoroutine(mainUI.FadeShowSprite());
                    
                    // FadeShow BottomBar
                    StartCoroutine(mainUI.FadeShowBottomBar());

                    // FadeChange Bottombar Color if the new sentence has the new character
                    yield return StartCoroutine( mainUI.FadeChangeSpeaker(
                        currentSceneStory.Sentences[sentenceIndex].character.characterName, 
                        currentSceneStory.Sentences[sentenceIndex].character.characterColor));

                    // Check if conversationType is Question;
                    if (currentSceneStory.Sentences[sentenceIndex].sentenceType ==
                        StoryScene.Sentence.SentenceType.Question)
                    {
                        //Debug.Log(sentenceIndex + " has sentence Type of Question");
                        
                        // Start typing conversation;
                        yield return StartCoroutine(
                        mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                        
                        // Start asking question;
                        yield return StartCoroutine(mainUI.AskQuestion(
                            currentSceneStory.Sentences[sentenceIndex].questionChoices));
                        
                        questionIndex = sentenceIndex;
                    }
                    
                    // Check if conversationType is Normal
                    else if (currentSceneStory.Sentences[sentenceIndex].sentenceType == 
                         StoryScene.Sentence.SentenceType.NormalConversation)
                    {
                        //Debug.Log(sentenceIndex + " has sentence Type of Normal");
                        
                        // Start typing conversation;
                        yield return StartCoroutine(
                        mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                        
                        questionIndex = sentenceIndex;
                    }
                    
                }
                
                // If character TranstionType is Normal
                else if (currentSceneStory.Sentences[sentenceIndex].characterTransitionType ==
                          StoryScene.Sentence.CharacterTransitionType.Normal)
                {
                    //Debug.Log(sentenceIndex + " has characterTransition Type of Normal");
                    
                    // Change component's color of the BottomBar;
                    mainUI.FadeChangeSpeaker(currentSceneStory.Sentences[sentenceIndex].character.characterName, 
                        currentSceneStory.Sentences[sentenceIndex].character.characterColor);
                    
                    // Change Sprite Emotion;
                    mainUI.ChangeSprite(currentSceneStory.Sentences[sentenceIndex].SpriteEmotion,
                        currentSceneStory.Sentences[sentenceIndex].character);
                    
                    // Check if conversationType is Question;
                    if (currentSceneStory.Sentences[sentenceIndex].sentenceType ==
                        StoryScene.Sentence.SentenceType.Question)
                    {
                        //Debug.Log(sentenceIndex + " has sentence Type of Question");
                        
                        // Start typing conversation;
                        yield return StartCoroutine(
                            mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                        
                        // Start asking question;
                        yield return StartCoroutine(mainUI.AskQuestion(
                            currentSceneStory.Sentences[sentenceIndex].questionChoices));
                        
                        questionIndex = sentenceIndex;
                    }
                    
                    // Check if conversationType is Normal
                    else if (currentSceneStory.Sentences[sentenceIndex].sentenceType == 
                             StoryScene.Sentence.SentenceType.NormalConversation)
                    {
                        //Debug.Log(sentenceIndex + " has sentence Type of Normal");
                        
                        // Start typing conversation;
                        yield return StartCoroutine(
                            mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                        
                        questionIndex = sentenceIndex;
                    }
                }
               
            }
            // delay before player can get to the next conversation;
            yield return new WaitForSeconds(NextConversationDelay);
            
            // update the conversation state;
            conversationState = ConversationState.COMPLEATED;
        }

        public void SwitchScene()
        {
            nextSceneStory = currentSceneStory.Sentences[sentenceIndex].questionChoices[choiceChosen].nextScene;
        }
        
        private IEnumerator StartStage()
        {
            // Check if the next sentence change place;
            currenntPlace = currentSceneStory.Sentences[0].Place;
            mainUI.SetBackground(currentSceneStory.Sentences[0].Place, 1);

            // Make sure nextSceneStory is null;
            nextSceneStory = null;
                
            // Set character Info on Bottombar before play the first sentence;
            mainUI.ChangeSpeaker(currentSceneStory.Sentences[0].character.characterName, 
                currentSceneStory.Sentences[0].character.characterColor);
            
            // set character sprite before play the first sentence;
            mainUI.ChangeSprite(currentSceneStory.Sentences[0].SpriteEmotion,
                currentSceneStory.Sentences[0].character );
            
            // Fade up Sprite & Bottombar;
            yield return StartCoroutine(mainUI.FadeShowSprite());
            yield return StartCoroutine(mainUI.FadeShowBottomBar());
            
            // Start Sound Staging;
            StartSoundStage();
            
            // Start GamePlay Staging;
            StartCoroutine(PlayNextSentence());
        }

        private IEnumerator EndStage(StoryScene story)
        {
            gameplayState = GamePlayState.ENDED;
            yield return null;
        }
        
        #endregion

        #region GamePlayManagement

        public enum GamePlayState
        {
            PLAYING,
            ENDED
        }
        [HideInInspector]
        public GamePlayState gameplayState;

        #endregion

        #region Sound Management
        
        void StartSoundStage()
        {
            // Stage Background Song
            bgSpeaker.clip = currentSceneStory.mainBG;
            bgSpeaker.loop = true;
            bgSpeaker.Play();
        }

        #endregion
        
        void Start()
        {
            // Find GamePlayUI in Scene
            mainUI = FindObjectOfType<GamePlayUI>();

            // Start GamePlay
            StartCoroutine(StartStage());
        }

        void Update()
        {
            
        }
    }
}
