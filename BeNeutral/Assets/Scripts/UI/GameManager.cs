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
        [SerializeField] private  SpwanPoint playerSpawnPoint; 
        //
        
        private int lifes = 3;
        private int level = 0;
        
        public void start()
        {
            //TODO
            startGame.LevelName = "Level1";
            StartGame();
            // start background music
            // start animations on the load screen
            
            //mychanges
            Debug.Log("game manager");
            SetupScene();
        }

        public void StartGame()
        {
            // get starting level
            startGame.LoadLevel();
            
        
            // TODO
            // - Who manages the level progression? 
        
            StartCoroutine(StartGameCoroutine());
        }

        IEnumerator StartGameCoroutine()
        {
            // TODO
            // start background music
            // AudioManager.Instance.StartBackgroundMusic();
        
            ClearUI();
        
            yield return new WaitForSeconds(1f);

            scoreDisplay.Open();
            // update the ui
            scoreDisplay.UpdateLifes(lifes);
        }

        private void ClearUI()
        {
            scoreDisplay.Close();
        }

        public void LoseLife()
        {
            // TODO
            // - play death sound
            // - deactivate both players
            // - update the display
            // - decrease the number of lifes
            // - if they have no more lifes then game over
            //   else start respawning both players from the last checkpoint
        }

        IEnumerator GameOverCoroutine()
        {
            // TODO
            // - wait a little bit
            yield return new WaitForSeconds(1f);
            // - disable the HUD
            scoreDisplay.Close();
            // - show the game over screen
                //SceneManager.LoadScene("GameOverScreen");
            yield return null;
        }
    
        IEnumerator Respawn()
        {
            // TODO 
            // reset the camera position
            // - reset players position and stats
            // - activate the two players

            yield return null;
        }

        public void ShowStartScreen()
        {
            // - clear all the UI
            ClearUI();
            // - activate the start screen
            SceneManager.LoadScene("InitialScreen");
        }

        public string ChooseLevel(int n)
        {
            string levelName;
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

        public void ShowNextLevel()
        {
            SceneManager.LoadScene("NextLevelScreen");
            scoreDisplay.UpdateLifes(lifes);
        }

        public void NextLevel()
        {
            // - Progress to the next level
            level = level + 1;
            startGame.LevelName = ChooseLevel(level);
            StartGame();
        }

        void Update()
        {
                
        }
        
        //myChanges
        public void SpawnPlayer()
        {
            if (playerSpawnPoint != null)
            {
                GameObject player = playerSpawnPoint.SpawnObject();
            }
        }

        public void SetupScene()
        {
            SpawnPlayer();
        }
    
    }
}

