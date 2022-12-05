using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Otome.UI;

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

        #endregion

        #region Story Management

        [SerializeField] StoryScene currentSceneStory;
        int sentenceIndex = -1;
        private State state = State.COMPLEATED;
        
        private enum State
        {
            PLAYING, COMPLEATED
        }

        public IEnumerator PlayNextSentence()
        {
            if (state == State.PLAYING)
            {
                yield break;
            }
            state = State.PLAYING;
            if (++sentenceIndex >= currentSceneStory.Sentences.Count)
            {
                yield break;
            }
            StartCoroutine(mainUI.TypeText(currentSceneStory.Sentences[sentenceIndex].conversationText));
            mainUI.ChangeSpeaker(currentSceneStory.Sentences[sentenceIndex].Character.characterName, currentSceneStory.Sentences[sentenceIndex].Character.characterColor);
            while (mainUI.typingState == GamePlayUI.TypingState.Playing)
            {
                yield return null;
            }

            yield return new WaitForSeconds(NextConversationDelay);
            state = State.COMPLEATED;
        }
        
        private IEnumerator StartStage(StoryScene story)
        {
            mainUI.ChangeSpeaker(currentSceneStory.Sentences[0].Character.characterName, currentSceneStory.Sentences[0].Character.characterColor);
            sentenceIndex = -1;
            yield return StartCoroutine(mainUI.FadeShowBottomBar());
            StartCoroutine(PlayNextSentence());
        }
        
        #endregion

        #region GamePlayManagement
        

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
