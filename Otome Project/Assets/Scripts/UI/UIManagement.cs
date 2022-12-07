using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Otome.UI
{
    public class UIManagement : MonoBehaviour
    {
        public void SwitchPanel(VisualElement toHide, VisualElement toShow)
        {
            toHide.pickingMode = PickingMode.Ignore;
            toHide.style.display = DisplayStyle.None;
            toShow.pickingMode = PickingMode.Position;
            toShow.style.display = DisplayStyle.Flex;
        }

        public enum LoadSceneTransitionType
        {
            Fade,
            FillBar
        }
        
        //public IEnumerator LoadScene(LoadSceneTransitionType transitionType)
        //{
            
        //}
    }
}
