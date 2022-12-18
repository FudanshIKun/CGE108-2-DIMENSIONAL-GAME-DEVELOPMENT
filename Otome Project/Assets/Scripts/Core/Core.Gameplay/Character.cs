using System;
using UnityEngine;

namespace Otome.Core
{
    [CreateAssetMenu(fileName = "NewCharacter", menuName = "Data/New Character")]
    [Serializable]
    public class Character : ScriptableObject
    {
        #region General Info
            [Header("General Info")]
            public string characterName;
            public enum GenderList
            {
                Male,Female,
                Bisexual
            }
            public GenderList characterGender;
            public Color characterColor;

        #endregion

        #region Sprite Info
            [Space(10)] [Header("Sprite List")]
            public Sprite normal;
            public Sprite smile;
            public Sprite angry;

        #endregion

        #region Data Info
            [Space(10)] [Header("Data")] 
            public string id;
            public int maxHeart;
            [HideInInspector] public int heartOwned;

            [ContextMenu("Generate Character ID")]
            private void GenerateGuid()
            {
                id = System.Guid.NewGuid().ToString();
            }

        #endregion
    }
}
