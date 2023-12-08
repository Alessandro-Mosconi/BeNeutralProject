using UnityEngine;

namespace UI
{
    public class NextLevel : MonoBehaviour
    {
        public void GoNextLevel()
        {
            GameManager.instance.NextLevel();
        }
        
    }
}
