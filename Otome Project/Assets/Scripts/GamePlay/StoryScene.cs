using System.Collections.Generic;
using UnityEngine;

namespace Otome.GamePlay
{
    [CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
    [System.Serializable]
    public class StoryScene : ScriptableObject
    {
        public List<Sentence> Sentences;
        public StoryScene nextScene;

        [System.Serializable]
        public struct Sentence
        {
            public string conversationText;

            public enum SentenceType
            {
                NormalConversation,
                Question
            }
            public SentenceType sentenceType;

            public Character character;

            public enum CharacterTransitionType
            {
                Normal,
                Fade
            }
            public CharacterTransitionType characterTransitionType;

            public enum EmotionList
            {
                normal,
                smile,
                angry
            }
            public EmotionList SpriteEmotion;
            public Texture2D Place;

            public enum BGTransitionType
            {
                normal,
                Fade
            }
            public BGTransitionType bgTransitionType;
            
            public AudioClip Soundfx;

        }
    }
}
