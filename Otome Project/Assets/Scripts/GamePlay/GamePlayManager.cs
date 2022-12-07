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

        public IEnumerator PlayNextSentence()
        {
            if (conversationState == ConversationState.PLAYING)
            {
                yield break;
            }
            conversationState = ConversationState.PLAYING;
            if (sentenceIndex + 1 >= currentSceneStory.Sentences.Count)
            {
                yield break;
            }

            if (currentSceneStory.Sentences[++sentenceIndex].Place != currenntPlace)
            {
                StartCoroutine(mainUI.SwitchBackground(currentSceneStory.Sentences[sentenceIndex].Place, currentSceneStory.Sentences[sentenceIndex].bgTransitionType));
                yield return StartCoroutine(mainUI.ChangeSprite(currentSceneStory.Sentences[sentenceIndex].SpriteEmotion,currentSceneStory.Sentences[sentenceIndex].character,
                    currentSceneStory.Sentences[sentenceIndex].characterTransitionType));
                currenntPlace = currentSceneStory.Sentences[sentenceIndex].Place; 
                StartCoroutine(mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                StartCoroutine( mainUI.FadeChangeSpeaker(currentSceneStory.Sentences[sentenceIndex].character.characterName, 
                    currentSceneStory.Sentences[sentenceIndex].character.characterColor));
            }
            else
            {
                
                StartCoroutine(mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                mainUI.FadeChangeSpeaker(currentSceneStory.Sentences[sentenceIndex].character.characterName, currentSceneStory.Sentences[sentenceIndex].character.characterColor);
                StartCoroutine(mainUI.ChangeSprite(currentSceneStory.Sentences[sentenceIndex].SpriteEmotion,currentSceneStory.Sentences[sentenceIndex].character,
                    currentSceneStory.Sentences[sentenceIndex].characterTransitionType));
            }

            while (mainUI.typingState == GamePlayUI.TypingState.Playing)
            {
                yield return null;
            }
            yield return new WaitForSeconds(NextConversationDelay);
            conversationState = ConversationState.COMPLEATED;
        }
        
        private IEnumerator StartStage(StoryScene story)
        {
            currenntPlace = currentSceneStory.Sentences[0].Place;
            mainUI.ChangeSpeaker(currentSceneStory.Sentences[0].character.characterName, currentSceneStory.Sentences[0].character.characterColor);
            StartCoroutine(mainUI.ChangeSprite(currentSceneStory.Sentences[0].SpriteEmotion,currentSceneStory.Sentences[0].character , 
                currentSceneStory.Sentences[0].characterTransitionType));
            sentenceIndex = -1;
            yield return StartCoroutine(mainUI.FadeShowSprite());
            yield return StartCoroutine(mainUI.FadeShowBottomBar());
            StartCoroutine(PlayNextSentence());
        }

        //private IEnumerator EndStage(StoryScene story)
        //{
            
        //}
        
        #endregion

        #region GamePlayManagement

        public enum GamePlayState
        {
            PLAYING,
            ENDED
        }

        #endregion
        
        void Start()
        {
            mainUI = FindObjectOfType<GamePlayUI>();
            StartCoroutine(StartStage(currentSceneStory));
        }

        void Update()
        {
            
        }
    }
}
