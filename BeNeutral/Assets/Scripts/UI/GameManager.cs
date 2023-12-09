using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameManager: Singleton<GameManager>
    {
        
            
        [Header("UI")]
        [SerializeField] private ScoreManager scoreDisplay;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private StartGame startGame;   
        // [SerializeField] private GameOverScreen gameOverScreen;
        
        //My changes
        [Header("PLAYER")]
        [SerializeField] private  SpwanPoint playerSpawnPoint; 
        [SerializeField] private  int startingLifes;
        //
        [Header("SCORES")]
        [SerializeField] private int levelPassedPoints;
        [SerializeField] private int enemieDestroyedPoints;
        [SerializeField] private int multiplierPoints;
        [SerializeField] private int damageLostPoints;
        [SerializeField] private int dieLostPoints;
        
        private int level = 0;
        
        
        public void Start()
        {
            //TODO
            startGame.LevelName = "Level1";
            scoreDisplay.SetLifes(startingLifes);
           
            scoreDisplay.Close();
            
            // start background music
            AudioManager.instance.StartBackgroundMusic();
            
            //TODO
            //start animations on the load screen
            
            //mychanges
            Debug.Log("game manager");
            
        }

        public void StartGame()
        {
            SetupScene();
            // get starting level
            startGame.LevelName = ChooseLevel(level);
            scoreDisplay.ResetScore();
            startGame.LoadLevel();
            
        
            StartCoroutine(StartGameCoroutine());
        }

        IEnumerator StartGameCoroutine()
        {
            // TODO
            // start background music for the game
            AudioManager.Instance.StartBackgroundGamingMusic();
        
            ClearUI();
            yield return new WaitForSeconds(1f);
            
            scoreDisplay.Open();
            // update the ui
        }

        private void ClearUI()
        {
            scoreDisplay.Close();
        }

        public void StartGameOver()
        {
            // If we want to start from the beginning of the game
            scoreDisplay.ResetScore();
            level = 0;
            ClearUI();
            StartCoroutine(StartGameOverCoroutine());
        }

        IEnumerator StartGameOverCoroutine()
        {
            // - wait a little bit
            yield return new WaitForSeconds(1f);
            // - disable the HUD
            scoreDisplay.Close();
            // - show the game over screen
            SceneManager.LoadScene("DeathScreeen");
            yield return new WaitForSeconds(10f);
            ShowStartScreen();
            yield return null;
        }
        
        public void ShowStartScreen()
        {
            // - clear all the UI
            ClearUI();
            // - activate the start screen
            SceneManager.LoadScene("InitialScreen");
            level = 0;
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
            startGame.LevelName = ChooseLevel(level);
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
            startGame.LevelName = ChooseLevel(level);
            StartGame(); 
        }
        
        public void SetupScene()
        {
            SpawnPlayer();
        }
        
        
        //myChanges
        public void SpawnPlayer()
        {
            if (playerSpawnPoint != null)
            {
                GameObject player = playerSpawnPoint.SpawnObject();
            }
        }
        
        public void KillPlayer()
        {
            audioManager.PlayDiePlayer();
            // Check if has more life
            if (scoreDisplay.LoseOneLife())
            {
                // If yes
                scoreDisplay.RestoreScore();
                scoreDisplay.SubToScore(dieLostPoints);
                RestartThisLevel(); 
            }
            else
            {
                // If not
                scoreDisplay.ResetScore();
                StartGameOver();
            }
        }

    }
}


