using UnityEngine;
using UnityEngine.UIElements;

namespace Otome.UI
{
    public class LevelMenu : MonoBehaviour
    {
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
        }
    }
}
