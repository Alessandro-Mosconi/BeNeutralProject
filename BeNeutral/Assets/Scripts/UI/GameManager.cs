using System.Collections;
using Codice.Client.BaseCommands.CheckIn;
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
        [SerializeField] private LoadingManager loadingManager;
        
        [Header("PLAYER")]
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
            
            // - start background music
            AudioManager.instance.ChooseBackgroundMusic(0);
            
            //TODO
            //start animations on the load screen
            
            //mychanges
            Debug.Log("game manager");
            
        }
        public void LoadLevel()
        {
            loadingManager.StartScene(LevelName);
            ObjectPoolingManager.Instance.RegenPools();
        }

        public void StartGame()
        {
            LoadLevel();
        }
        public void ReloadLevel()
        {
            loadingManager.RestartScene(LevelName);
            ObjectPoolingManager.Instance.RegenPools();
        }
        public void RestartGame()
        {
            ReloadLevel();
        }
        
        public void ResetGame()
        {
            ObjectPoolingManager.Instance.ResetPools();
            ClearUI();
            LevelName = "Level3";
            scoreDisplay.SetLifes(startingLifes);
            scoreDisplay.ResetScore();
        }
        private void ClearUI()
        {
            scoreDisplay.Close();
        }

        public void StartGameOver()
        {
            // - If we want to start from the beginning of the game
            ResetGame();
            StartCoroutine(StartGameOverCoroutine());
            // - Background music set to Losing music
            audioManager.StopBackgroundMusic();
            audioManager.PlayGameOver();
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
            // - background music set to Main Menu music
            audioManager.ChooseBackgroundMusic(0);
        }

        public string ChooseLevel(int n)
        {
            string levelName;
            // - Select the level name corresponding to the level number
            switch (n)
            {
                case 0:
                    levelName = "LevelTutorial";
                    break;
                case 1:
                    levelName = "Level1";
                    break;
                case 2:
                    levelName = "Level2";
                    break;
                case 3:
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
            // - Show next level scene with button to proceed in the next game level
            SceneManager.LoadScene("NextLevelScreen");
            // - set music to Next level screen music
            audioManager.ChooseBackgroundMusic(3);
            // - giving points for winning the level
            scoreDisplay.AddToScore(levelPassedPoints);
            // - checkpoint for the score
            scoreDisplay.SaveLastScore();
        }
        
        public void RestartThisLevel()
        {
            // - Restart after die from the same level
            LevelName = ChooseLevel(level);
            RestartGame(); 
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
            // - death sound of player
            // - audioManager.PlayDiePlayer();
            scoreDisplay.SubToScore(damageLostPoints);
        }
        public void KillEnemie(int points)
        {
            //TODO
            // - death sound of the enemie
            audioManager.PlayDieEnemie();
            scoreDisplay.AddToScore(points);
        }
        public void KillPlayer()
        {
            // - death sound of the player
            audioManager.PlayDiePlayer();
            // - Check if has more life
            if (scoreDisplay.LoseOneLife())
            {
                // - If yes
                scoreDisplay.RestoreScore();
                scoreDisplay.SubToScore(dieLostPoints);
                scoreDisplay.SaveLastScore();
                RestartThisLevel(); 
            }else{
                // - If not
                scoreDisplay.ResetScore();
                StartGameOver();
            }
        }

    }
}


