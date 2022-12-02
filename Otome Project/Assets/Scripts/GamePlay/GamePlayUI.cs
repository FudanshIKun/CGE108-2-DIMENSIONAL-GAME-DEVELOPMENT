using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Otome.GamePlay
{
    public class GamePlayUI : MonoBehaviour
    {
        public Sprite Test;
        public VisualElement root;
        
        #region UI Management
        
        private Button pauseButton;        
        
        #endregion
                
        #region Background Management
        
        private int currentBG = 1;
        
        private VisualElement bg01;
        private VisualElement bg02;
        
        public void ChangeBackground(int numberOfBG ) // *** Using in GamePlayManager ***
        {
            switch (numberOfBG)
            {
                case 1 :
                    Debug.Log("Change Background number 0");
                    break;
                case 2 :
                    Debug.Log("Change Background number 1");
                    break;
                default :
                    break;
            }
                    
        }
        public void SwitchBackground() // *** Using in GamePlayManager ***
        {
            switch (currentBG)
            {
                case 1 : 
                    Debug.Log("Set BG01's oppacity to 0");
                    currentBG = 2; 
                    break;
                case 2 :
                    Debug.Log("Set BG01's oppacity to 100");
                    currentBG = 1;
                    break;
                default :
                    break;
            }
        }
        
        #endregion
        
        #region BottomBar Management
                
        public enum ConversationState
        { 
            Playing,
            Compleated
        }
        
        private GroupBox bottomBar;
        private VisualElement bar;
        private VisualElement circle;
        private Label conversationText;
        private Label personNameText;

        // private IEnumerator ShowBottomBar() // *** Using in GamePlayManager ***
        // {
          
        // }
                
        // private IEnumerator HideBottomBar() // *** Using in GamePlayManager ***
        // {
            
        // }
        
        // private IEnumerator TypeText(string text) // *** Using in GamePlayManager ***
        // {
           
        // }
        
        #endregion
        
        #region Character Management
        
        private VisualElement CharacterSprite;
        
        public void SetSprite(Sprite targetSprite) // *** Using in GamePlayManager ***
        { 
            CharacterSprite.style.backgroundImage = new StyleBackground(targetSprite);
        }
        
        #endregion
        
        void AddFunctionUI()
        {
            
        }
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            bg01 = root.Q<VisualElement>("Background01");
            bg02 = root.Q<VisualElement>("Background02");
            bottomBar = root.Q<GroupBox>("BottomBar");
            bar = bottomBar.ElementAt(0);
            circle = bottomBar.ElementAt(1);
            conversationText = root.Q<Label>("ConversationText");
            pauseButton = root.Q<Button>("PauseButton");
            CharacterSprite = root.Q<VisualElement>("Character");
        }
    }
}
