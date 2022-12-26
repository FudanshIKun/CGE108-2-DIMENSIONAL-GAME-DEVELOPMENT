using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Otome.Core
{
    [CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
    [Serializable]
    public class StoryScene : ScriptableObject
    {
        public string id;
        public List<Character> Characters; 
        public List<Sentence> Sentences;
        [ContextMenu("Generate Episode ID")]
        private void GenerateGuid()
        {
            id = Guid.NewGuid().ToString();
        }

        [Serializable]
        public struct Sentence
        {
            #region Conversation Management
                [Space(3)] [Header(("Sentence Setting"))]

                public string conversationText;
                public SentenceType sentenceType;
                public List<ChoiceLabel> questionChoices;

                public enum SentenceType
                {
                    NormalConversation,
                    Question,
                    EndSentence
                }

            #endregion
            

            #region Character Management
            
                [Space(3)] [Header("Character Setting")]

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

            #region Background Management
                [Space(3)] [Header(("Background Setting"))]

                public Texture2D Place;
                public BGTransitionType bgTransitionType;
                public enum BGTransitionType
                {
                    bg_NormalCharacter_Normal,
                    bg_NormalCharacter_Fade,
                    bg_FadeCharacter_Normal
                }

            #endregion

            #region Sound Management
                [Space(3)] [Header(("Sound Setting"))] 
            
                public AudioClip newBGSong;
                public AudioClip conversationSFX;

            #endregion

            
        }
        
        [Serializable]
        public struct ChoiceLabel
        {
            public string text;
            public StoryScene nextScene;
            public bool rightAnswer;
            public bool normalAnswer;
            public bool badAnswer;

            public void StoreAnswer()
            {
                
            }
        }
    
    }
}
