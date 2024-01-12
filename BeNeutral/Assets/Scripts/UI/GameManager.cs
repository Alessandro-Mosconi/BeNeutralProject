using System;
using System.Collections;
using Codice.Client.BaseCommands.CheckIn;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class GameManager: Singleton<GameManager>
    {
        
            
        [Header("UI")]
        [SerializeField] private ScoreManager scoreDisplay;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private LoadingManager loadingManager;
        [SerializeField] private Animations animator;
        
        
        [Header("INTRO")]
        [SerializeField] private Canvas introGroup;
        [SerializeField] private VideoPlayer videoIntro;
        [SerializeField] private CanvasGroup introText;
        [SerializeField] private TMP_Text introTextValue;
        
        [Header("MENU")]
        [SerializeField] private MenuManager menuManager;
        [SerializeField] private CanvasGroup menuManagerGroup;
        [SerializeField] private TMP_Text titleMainMenu;
        
        [Header("PLAYER")]
        [SerializeField] private  int startingLifes;
        
        [Header("SCORES")]
        [SerializeField] private int levelPassedPoints;
        [SerializeField] private int multiplierPoints;
        [SerializeField] private int damageLostPoints;
        [SerializeField] private int dieLostPoints;
        
        [Header("STARTING OPTIONS")]
        [SerializeField] private int level = 0;
        private string _levelName;
        private bool _inGame = false;
        
        public void Start()
        {
             ResetGame();
             StartCoroutine(updateDuringGame());
             PlayIntro();
             
            //TODO
            // - start animations on the load screen

        }

        public void PlayIntro()
        {
            Coroutine fadeIn = StartCoroutine(introFadeIn());
            videoIntro.Play();
            StartCoroutine(introFadeOut(fadeIn));
        }
        // - special fade in for video and music
        IEnumerator introFadeIn()
        {
            float x = 0.0001f;
            while(videoIntro.targetCameraAlpha < 0.6f)
            {
                videoIntro.targetCameraAlpha += x;
                videoIntro.SetDirectAudioVolume(0,videoIntro.GetDirectAudioVolume(0)+x);
                x += 0.0001f;
                yield return new WaitForSeconds(0.05f);
            }
        }
        
        IEnumerator introFadeOut(Coroutine fadeIn)
        {
            yield return new WaitForSeconds(1.5f);
            if (videoIntro.isPlaying)
            {
                animator.FadeIn(introText,0.5f);
                animator.TypeWriterText("Press F to skip the intro...",introTextValue,8f, false);
            }
            while (videoIntro.isPlaying)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    StopCoroutine(fadeIn);
                    break;
                }

                yield return null;
            }
            while (!animator._typeWriterEnded)
            {
                yield return null;
            }
            StartCoroutine(startMenu());
            animator.FadeOut(introText,1f);
            while(videoIntro.targetCameraAlpha > 0)
            {
                float x = 0.003f;
                videoIntro.targetCameraAlpha -= x;
                videoIntro.SetDirectAudioVolume(0,videoIntro.GetDirectAudioVolume(0)-x);
                yield return new WaitForSeconds(0.008f);
            }
            introGroup.gameObject.SetActive(false);
            
            
        }

        IEnumerator startMenu()
        {
            menuManager.gameObject.SetActive(true);
            AudioManager.instance.ChooseBackgroundMusic(0);
            
            float y = 0f;
            
            while (y <= 1f)
            {
                menuManagerGroup.alpha = y;
                y +=  0.002f;
                yield return new WaitForSeconds(0.005f);
            }
        }
        public void LoadLevel()
        {
            _inGame = true;
            loadingManager.StartScene(_levelName);
            ObjectPoolingManager.Instance.RegenPools();
        }

        public void StartGame()
        {
            LoadLevel();
        }
        public void ReloadLevel()
        {
            loadingManager.RestartScene(_levelName);
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
            _levelName = ChooseLevel(level);
            scoreDisplay.SetLifes(startingLifes);
            scoreDisplay.ResetScore();
            UnpauseGame();
            _inGame = false;
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
            level = 0;
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
            // - reset level
            level = 0;
            // - reactivate menu
            MenuManager.instance.OpenMainMenu();
            // - background music set to Main Menu music
            audioManager.ChooseBackgroundMusic(0);
        }

        public string ChooseLevel(int n)
        {
            string levelName;
            // - Select the level name corresponding to the level number
            switch (n)
            {
                case -1:
                    levelName = "TestCutScene";
                    break;
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
            _levelName = ChooseLevel(level);
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
            _levelName = ChooseLevel(level);
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
        public void KillEnemy(int points)
        {
            //TODO
            // - death sound of the enemy
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

        public void PauseGame()
        {
            // TODO
            // commands to pause the 
            Time.timeScale = 0;
        }

        public void UnpauseGame()
        {
            // TODO
            // commands to restart the game from the pause point
            Time.timeScale = 1;
        }

        IEnumerator updateDuringGame()
        {
            while (true)
            {
                if (_inGame)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (menuManager.gameMenuOpened())
                        {
                            menuManager.CloseMenu();
                            UnpauseGame();
                        }
                        else
                        {
                            menuManager.OpenGameMenu();
                            PauseGame();
                        }
                    }
                }
                yield return null; 
            }
            
        }
        

    }
}


