using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace UI
{
    public class GameManager: Singleton<GameManager>
    {
        
            
        [Header("UI")]
        [SerializeField] private ScoreManager scoreDisplay;
        [SerializeField] private AudioManager audioManager;
        
        //My changes
        [Header("PLAYER")]
        //[SerializeField] private  SpwanPoint playerSpawnPoint; 
        [SerializeField] private  int startingLifes;
        //
        [Header("SCORES")]
        [SerializeField] private int levelPassedPoints;
        [SerializeField] private int multiplierPoints;
        [SerializeField] private int damageLostPoints;
        [SerializeField] private int dieLostPoints;
        
        private int level = 0;
        private string LevelName;
        
        
        public void Start()
        {
            ResetGame();
            
            // start background music
            AudioManager.instance.StartBackgroundMusic();
            
            //TODO
            //start animations on the load screen
            
            //mychanges
            Debug.Log("game manager");
            
        }
        public void LoadLevel()
        {
            SceneManager.LoadScene(LevelName);
        }

        public void StartGame()
        {
            //SetupScene();
            // get starting level
            LoadLevel();

            StartCoroutine(StartGameCoroutine());
        }

        IEnumerator StartGameCoroutine()
        {
            // TODO
            // start background music for the game
            //AudioManager.Instance.StartBackgroundGamingMusic();
        
            ClearUI();
            yield return new WaitForSeconds(0.3f);
            scoreDisplay.Open();
        }
        public void ResetGame()
        {
            ClearUI();
            LevelName = "Level1";
            scoreDisplay.SetLifes(startingLifes);
            scoreDisplay.ResetScore();
        }
        private void ClearUI()
        {
            scoreDisplay.Close();
        }

        public void StartGameOver()
        {
            // If we want to start from the beginning of the game
            ResetGame();
            StartCoroutine(StartGameOverCoroutine());
        }

        IEnumerator StartGameOverCoroutine()
        {
            // - show the game over screen
            SceneManager.LoadScene("DeathScreen");
            yield return null;
        }
        
        public void ShowStartScreen()
        {
            // - Reset game from the beginning
            ResetGame();
            // - activate the start screen
            SceneManager.LoadScene("InitialScreen");
            
        }

        public string ChooseLevel(int n)
        {
            string levelName;
            // Select the level name corresponding to the level number
            switch (n)
            {
                case 0:
                    levelName = "Level1";
                    break;
                case 1:
                    levelName = "Level2";
                    break;
                case 2:
                    levelName = "Level3";
                    break;
                default:
                    levelName = "InitialScreen";
                    break;
            }
            return levelName;
        }
        public void NextLevel()
        {
            // - Progress to the next level
            level = level + 1;
            LevelName = ChooseLevel(level);
            StartGame();
        }
        public void ShowNextLevel()
        {
            // Show next level scene with button to proceed in the next game level
            SceneManager.LoadScene("NextLevelScreen");
            scoreDisplay.AddToScore(levelPassedPoints);
            // checkpoint for the score
            scoreDisplay.SaveLastScore();
        }
        
        public void RestartThisLevel()
        {
            // Restart after die from the same level
            LevelName = ChooseLevel(level);
            StartGame(); 
        }

        public int GetStartingLifes()
        {
            return startingLifes;
        }
        

        //public void SetupScene()
        //{
         //   SpawnPlayer();
        //}
        
        
        //myChanges
        //public void SpawnPlayer()
        //{
        //    if (playerSpawnPoint != null)
        //    {
        //        GameObject player = playerSpawnPoint.SpawnObject();
        //    }
        //}

        public void TakeDamage()
        {
            //TODO
            //death sound of player
            //audioManager.PlayDiePlayer();
            scoreDisplay.SubToScore(damageLostPoints);
        }
        public void KillEnemie(int points)
        {
            //TODO
            //death sound of enemie
            //audioManager.PlayDieEnemie();
            scoreDisplay.AddToScore(points);
        }
        public void KillPlayer()
        {
            // Check if has more life
            if (scoreDisplay.LoseOneLife())
            {
                // If yes
                scoreDisplay.RestoreScore();
                scoreDisplay.SubToScore(dieLostPoints);
                scoreDisplay.SaveLastScore();
                RestartThisLevel(); 
            }else{
                // If not
                scoreDisplay.ResetScore();
                StartGameOver();
            }
        }

    }
}


