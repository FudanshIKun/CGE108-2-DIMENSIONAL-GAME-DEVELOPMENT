using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

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
