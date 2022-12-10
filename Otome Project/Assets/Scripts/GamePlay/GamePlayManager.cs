using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Otome.UI;
using UnityEngine.UIElements;

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
        [SerializeField] float NextConversationDelay;
        [Space(5)]
        [Header("Audio Setting")]
        public AudioSource bgSpeaker;
        public AudioSource sfxSpeaker;
        [Space(5)]
        [Header("GamePlay Setting")]
        public GamePlayState gameState = GamePlayState.PLAYING;
        [HideInInspector]
        public bool isPaused;
        
        #region UI management

        private GamePlayUI mainUI;
        
        #endregion

        #region Story Management

        [SerializeField] StoryScene currentSceneStory;
        int sentenceIndex = -1;
        private enum ConversationState
        {
            PLAYING, COMPLEATED
        }
        private ConversationState conversationState = ConversationState.COMPLEATED;
        private Texture2D currenntPlace;
        private Character currentCharacter;

        public IEnumerator PlayNextSentence()
        {
            // Check if the conversation hasn't end but player call this function;
            if (conversationState == ConversationState.PLAYING)
            {
                yield break;
            }
            
            // if the conversation has COMPLEATE set it to PLAYING when player call this function again;
            conversationState = ConversationState.PLAYING;
            
            // Check if there is no next sentence in the index then break;
            if (sentenceIndex + 1 >= currentSceneStory.Sentences.Count)
            {
                yield break;
            }
            
            // Check if the sentence we gonna play change the location
            if (currentSceneStory.Sentences[++sentenceIndex].Place != currenntPlace)
            {
                // Switch Background if the new sentence has new location;
                yield return StartCoroutine(mainUI.SwitchBackground(
                    currentSceneStory.Sentences[sentenceIndex].Place, 
                    currentSceneStory.Sentences[sentenceIndex].bgTransitionType));
                yield return new WaitForSeconds(1f);
                
                // Change character sprite by the transition type setting in sentence;
                yield return StartCoroutine(mainUI.ChangeSprite(
                    currentSceneStory.Sentences[sentenceIndex].SpriteEmotion,
                    currentSceneStory.Sentences[sentenceIndex].character,
                    currentSceneStory.Sentences[sentenceIndex].characterTransitionType));
                
                // update the currentPlace for check if there's new location;
                currenntPlace = currentSceneStory.Sentences[sentenceIndex].Place;
                
                // FadeChange Bottombar Color if the new sentence has the new character
                StartCoroutine( mainUI.FadeChangeSpeaker(
                    currentSceneStory.Sentences[sentenceIndex].character.characterName, 
                    currentSceneStory.Sentences[sentenceIndex].character.characterColor));

                if (currentSceneStory.Sentences[sentenceIndex].sentenceType ==
                    StoryScene.Sentence.SentenceType.Question)
                {
                    StartCoroutine(mainUI.AskQuestion());
                }
                else if (currentSceneStory.Sentences[sentenceIndex].sentenceType == 
                         StoryScene.Sentence.SentenceType.NormalConversation)
                {
                    // Start typing conversation;
                    yield return StartCoroutine(
                        mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                }
            }
            else // if the new sentence has the same location as the previous one
            {
                // Start typing conversation;
                StartCoroutine(mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                
                // Change character sprite by the transition type setting in sentence;
                mainUI.FadeChangeSpeaker(currentSceneStory.Sentences[sentenceIndex].character.characterName, 
                    currentSceneStory.Sentences[sentenceIndex].character.characterColor);
                // Change BottomBarColor & Speaker Info;
                yield return StartCoroutine(mainUI.ChangeSprite(
                    currentSceneStory.Sentences[sentenceIndex].SpriteEmotion,
                    currentSceneStory.Sentences[sentenceIndex].character,
                    currentSceneStory.Sentences[sentenceIndex].characterTransitionType));
            }
            // delay before player can get to the next conversation;
            yield return new WaitForSeconds(NextConversationDelay);
            // update the conversation state;
            conversationState = ConversationState.COMPLEATED;
        }
        
        private IEnumerator StartStage(StoryScene story)
        {
            // Check if the next sentence change place;
            currenntPlace = currentSceneStory.Sentences[0].Place;
            // Set character Info on Bottombar before play the first sentence;
            mainUI.ChangeSpeaker(currentSceneStory.Sentences[0].character.characterName, 
                currentSceneStory.Sentences[0].character.characterColor);
            // set character sprite before play the first sentence;
            StartCoroutine(mainUI.ChangeSprite(currentSceneStory.Sentences[0].SpriteEmotion,
                currentSceneStory.Sentences[0].character , 
                currentSceneStory.Sentences[0].characterTransitionType));
            sentenceIndex = -1;
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
            StartCoroutine(StartStage(currentSceneStory));
        }

        void Update()
        {
            
        }
    }
}
