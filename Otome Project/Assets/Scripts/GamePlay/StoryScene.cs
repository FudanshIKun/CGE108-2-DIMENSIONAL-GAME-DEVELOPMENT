using System.Collections.Generic;
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

        [System.Serializable]
        public struct Sentence
        {
            #region Conversation Info

            public string conversationText;

            public enum SentenceType
            {
                NormalConversation,
                Question
            }
            public SentenceType sentenceType;

            #endregion

            #region Character Info

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

            #endregion

            #region Background Info

            public Texture2D Place;
            public enum BGTransitionType
            {
                BGnormal_CharacterNormal,
                BGnormal_CharacterFade,
                BGFade_CharacterNormal
            }
            public BGTransitionType bgTransitionType;

            #endregion

            #region Sounds Info

            public AudioClip conversationSFX;

            #endregion

        }
    }
}
