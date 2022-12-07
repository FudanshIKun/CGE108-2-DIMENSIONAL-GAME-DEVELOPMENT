using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Otome.GamePlay
{
    [CreateAssetMenu(fileName = "NewCharacter", menuName = "Data/New Character")]
    [System.Serializable]
    public class Character : ScriptableObject
    {
        [Header("General Info")]
        public string characterName;
        public enum GenderList
        {
            Male,Female,
            Bisexual
        }
        public GenderList characterGender;
        public Color characterColor;
        
        [Space(10)] 
        [Header("Sprite List")] 
        public Sprite normal;
        public Sprite smile;
        public Sprite angry;
    }
}
