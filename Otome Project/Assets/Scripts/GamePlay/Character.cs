using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Otome.GamePlay
{
    [CreateAssetMenu(fileName = "NewCharacter", menuName = "Data/New Character")]
    [System.Serializable]
    public class Character : ScriptableObject
    {
        public string characterName;
        public Color textColor;
        [Space(10)] 
        [Header("Sprite List")] 
        public Sprite normal;
        public Sprite smile;
        public Sprite angry;
    }
}
