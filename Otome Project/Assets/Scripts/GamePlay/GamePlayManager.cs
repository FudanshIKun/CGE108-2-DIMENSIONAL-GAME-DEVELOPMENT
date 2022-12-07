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

        [Header("Setting")] 
        [Range(1f, 2f)] 
        [SerializeField] float NextConversationDelay;
        
        #region UI management

        private GamePlayUI mainUI;

        void ChangeEmotion(Character character,StoryScene.Sentence.EmotionList emotion)
        {
            switch (emotion)
            {
                case StoryScene.Sentence.EmotionList.normal :
                    mainUI.ChangeSprite(character.normal);
                    break;
                case StoryScene.Sentence.EmotionList.smile :
                    mainUI.ChangeSprite(character.smile);
                    break;
                case StoryScene.Sentence.EmotionList.Angry :
                    mainUI.ChangeSprite(character.angry);
                    break;
            }
        }
        
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
                yield return StartCoroutine(mainUI.SwitchBackgroundWithoutFade(currentSceneStory.Sentences[sentenceIndex].Place));
                currenntPlace = currentSceneStory.Sentences[sentenceIndex].Place;
                StartCoroutine(mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                mainUI.ChangeSpeaker(currentSceneStory.Sentences[sentenceIndex].character.characterName, currentSceneStory.Sentences[sentenceIndex].character.characterColor);
                ChangeEmotion(currentSceneStory.Sentences[sentenceIndex].character,currentSceneStory.Sentences[sentenceIndex].SpriteEmotion);
            }
            else
            {
                StartCoroutine(mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
                mainUI.ChangeSpeaker(currentSceneStory.Sentences[sentenceIndex].character.characterName, currentSceneStory.Sentences[sentenceIndex].character.characterColor);
                ChangeEmotion(currentSceneStory.Sentences[sentenceIndex].character,currentSceneStory.Sentences[sentenceIndex].SpriteEmotion);
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

        public bool isPaused;

        public enum GamePlayState
        {
            PLAYING,
            ENDED
        }
        [HideInInspector]
        public GamePlayState gameState = GamePlayState.PLAYING;

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
