using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Managers
{
    public class StartGame : MonoBehaviour
    {
        public string LevelName;

        public void Awake()
        {
          ScoreManager.instance.Close();
        }
        
        public void LoadLevel()
        {
            SceneManager.LoadScene(LevelName);
        }

        public void StartNewGame()
        {
            GameManager.instance.start();
        }
    }
}
