using UnityEngine;

namespace UI
{
    public class StartButton : MonoBehaviour
    {
    
        public void OnClick()
        {
            GameManager.instance.StartGame();
        }
    
    }
}
