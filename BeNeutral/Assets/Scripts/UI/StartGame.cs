using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class StartGame : MonoBehaviour
    {
        public string LevelName;
        
        public void LoadLevel()
        {
            SceneManager.LoadScene(LevelName);
        }

        public void StartNewGame()
        {
            GameManager.instance.StartGame();
        }
    }
}

