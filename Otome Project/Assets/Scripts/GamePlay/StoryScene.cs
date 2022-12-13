using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Otome.GamePlay
{
    [CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
    [System.Serializable]
    public class StoryScene : ScriptableObject
    {
        public AudioClip mainBG;
        public StoryScene nextScene;
        public List<Sentence> Sentences;

        [Serializable]
        public struct Sentence
        {
            #region Conversation Info
            [Space(3)]
            [Header(("Sentence Setting"))]

            public string conversationText;
            public SentenceType sentenceType;
            public List<ChoiceLabel> questionChoices;

            public enum SentenceType
            {
                NormalConversation,
                Question
            }

            #endregion

            #region BottomBar

            //[Space(3)]
            //[Header("BottomBar Setting")]
            

            #endregion

            #region Character
            [Space(3)]
            [Header("Character Setting")]

            public Character character;
            public CharacterTransitionType characterTransitionType;
            public EmotionList SpriteEmotion;

            public enum CharacterTransitionType
            {
                Normal,
                Fade
            }

            public enum EmotionList
            {
                normal,
                smile,
                angry
            }

            #endregion

            #region Background
            [Space(3)]
            [Header(("Background Setting"))]

            public Texture2D Place;
            public BGTransitionType bgTransitionType;
            public enum BGTransitionType
            {
                bg_NormalCharacter_Normal,
                bg_NormalCharacter_Fade,
                bg_FadeCharacter_Normal
            }

            #endregion

            #region Sound
            [Space(3)] 
            [Header(("Sound Setting"))] 
            
            AudioClip newBGSong;
            public AudioClip conversationSFX;

            #endregion

            
        }
        [Serializable]
        public struct ChoiceLabel
        {
            public string text;
            public StoryScene nextScene;

            public void StoreAnswer()
            {
                
            }
        }
    
    }
}
