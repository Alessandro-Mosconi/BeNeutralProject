using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Video;
using Random = System.Random;

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
        [SerializeField] private CanvasGroup introImage;
        private bool _endIntroImage = false;
        
        [Header("MENU")]
        [SerializeField] private MenuManager menuManager;
        [SerializeField] private CanvasGroup menuManagerGroup;
        [SerializeField] private TMP_Text titleMainMenu1;
        [SerializeField] private TMP_Text titleMainMenu2;
        [SerializeField] private TMP_Text titleMainMenu3;

        [Header("PLAYER")]
        [SerializeField] private  int startingLives;
        
        // - general stats player
        
        private float _maxHitPoints;
        private float _startingHitPoints;
        private float _fallDamageValue;
        private float _hazardDamageValue;
        private float _continuousDamageValue;
        private float _playerDamage;
        
        [Header("SCORES")]
        [SerializeField] private int levelPassedPoints;
        [SerializeField] private int multiplierPoints;
        [SerializeField] private int damageLostPoints;
        [SerializeField] private int dieLostPoints;
        
        [Header("STARTING OPTIONS")]
        [SerializeField] private int level;
        private string _levelName;
        private bool _inGame;

        [FormerlySerializedAs("animationTimeIntroFadeIn")]
        [Header("ANIMATIONS")]
        [SerializeField] private float animationSpeedIntroFadeIn;
        [SerializeField] private float animationSpeedIntroFadeOut;
        [SerializeField] private float animationSpeedMenuFadeIn;
        [SerializeField] private int animationSpeedMenuTitle;

        private float standardAnimationYieldTime = 0.0001f;
        private float _initialTimeLevel;
        private float _endingTimeLevel;

        private GameObject _coinPrefab;
        private bool _isPlayerDead = false;
        
        // - getter for parameters

        public float GetMaxHitPoints()
        {
            return _maxHitPoints;
        }
        public float GetStartingHitPoints()
        {
            return _startingHitPoints;
        }
        public float GetFallDamageValue()
        {
            return _fallDamageValue;
        }
        public float GetHazardDamageValue()
        {
            return _hazardDamageValue;
        }
        public float GetContinuousDamageValue()
        {
            return _continuousDamageValue;
        }
        public float GetPlayerDamage()
        {
            return _playerDamage;
        }
        public bool GetIsPlayerDead()
        {
            return _isPlayerDead;
        }
        public void SetIsPlayerDead(bool value)
        {
            print("setto a " + value);
            _isPlayerDead = value;
        }
        
        public void SetParameters(int lives, int levelPoint, int damageLostPoint, int dieLostPoint, float maxHitPoints, float startingHitPoints, float fallDamageValue, float hazardDamageValue, float continuousDamageValue, float playerDamage)
        {
            startingLives = lives;
            levelPassedPoints = levelPoint;
            damageLostPoints = damageLostPoint;
            dieLostPoints = dieLostPoint;
            _maxHitPoints = maxHitPoints;
            _startingHitPoints = startingHitPoints;
            _fallDamageValue = fallDamageValue;
            _hazardDamageValue = hazardDamageValue; 
            _continuousDamageValue = continuousDamageValue;
            _playerDamage = playerDamage;
        }
        
        public void Start()
        {
             ResetGame();
            
             // - coin prefab initialization for spawning points during the game
             
             _coinPrefab = Resources.Load<GameObject>("Coin Variant");
             ObjectPoolingManager.Instance.CreatePool (_coinPrefab, 50, 100);
             
             // - update during the all game
             
             StartCoroutine(UpdateDuringGame());
             
             // - start intro video with fade in and fade out
             
             PlayIntro();

        }

        private void PlayIntro()
        {
            // - play Polimi Game Collective introduction

            StartCoroutine(IntroImage());
            
            // - play game intro

            StartCoroutine(Intro());
            
        }
        // - Polimi game collective image
        IEnumerator IntroImage()
        {
            _endIntroImage = false;
            animator.FadeIn(introImage, 3f);
            while (introImage.alpha < 1f)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(2f);
            
            animator.FadeOut(introImage, 0.6f);
            while (introImage.alpha > 0.1f)
            {
                yield return null;
            }
            _endIntroImage = true;
            yield return null;
        }

        IEnumerator Intro()
        {
            while (!_endIntroImage)
            {
                yield return null;
            }
            // - fade in for the intro video
            
            Coroutine fadeIn = StartCoroutine(IntroFadeIn());
            
            // - video intro start to play
            
            videoIntro.Play();
            
            // - fade out when the video ended or when skipped
            
            StartCoroutine(IntroFadeOut(fadeIn));
            yield return null;
        }
        // - fade in for video and music
        IEnumerator IntroFadeIn()
        {
            float x = animationSpeedIntroFadeIn/10000;
            while(videoIntro.targetCameraAlpha < 0.6f)
            {
                videoIntro.targetCameraAlpha += x;
                videoIntro.SetDirectAudioVolume(0,videoIntro.GetDirectAudioVolume(0)+x);
                x += animationSpeedIntroFadeIn/10000;
                yield return new WaitForSeconds(standardAnimationYieldTime);
            }
        }
        
        // - fade out for video and music
        bool skipIntroText = false;

        IEnumerator IntroFadeOut(Coroutine fadeIn)
        {
            while (!videoIntro.isPlaying)
            {
                yield return null;
            }
            
            if (videoIntro.isPlaying)
            {
                animator.FadeIn(introText, 0.5f);
                animator.TypeWriterText("Press F to skip the intro...", introTextValue, 8f, false);
        
                // Check for skip input while typing
                while (!Input.GetKeyDown(KeyCode.F))
                {
                    if (!videoIntro.isPlaying)
                        break;
                
                    yield return null;
                }
        
                // If F is pressed, set skipIntroText to true
                if (Input.GetKeyDown(KeyCode.F))
                {
                    skipIntroText = true;
                }
            }

            // - start the menuScreen with the buttons 
            StartCoroutine(StartMenu());
    
            // - fade out of the text
            animator.FadeOut(introText, 1f);
    
            // - fade out of the video and music
            while (videoIntro.targetCameraAlpha > 0)
            {
                float x = animationSpeedIntroFadeOut / 100;
                videoIntro.targetCameraAlpha -= x;
                videoIntro.SetDirectAudioVolume(0, videoIntro.GetDirectAudioVolume(0) - x);
                yield return new WaitForSeconds(standardAnimationYieldTime);
            }
            introGroup.gameObject.SetActive(false);
        }

        void Update()
        {
            // If skipIntroText is true, stop the text typing immediately
            if (introTextValue!= null && skipIntroText)
            {
                // Implement logic to stop the text typing immediately
                // For example, you can set introTextValue.text to the full text
                introTextValue.text = "Press F to skip the intro...";
            }
        }


        IEnumerator StartMenu()
        {
            bool titleStarted = false;
            menuManager.gameObject.SetActive(true);
            AudioManager.instance.ChooseBackgroundMusic(0);
            
            float y = 0f;
            
            // - fade in of the start menu
            while (y <= 1f)
            {
                // - title effect with separated animation
                if (y > 0.2f && !titleStarted)
                {
                    titleStarted = true;
                    animator.GrowingTextAnimation("Be", titleMainMenu1, 80, animationSpeedMenuTitle);
                    animator.GrowingTextAnimation("Neu", titleMainMenu2, 80, animationSpeedMenuTitle);
                    animator.GrowingTextAnimation("tral", titleMainMenu3, 80, animationSpeedMenuTitle);
                }
                menuManagerGroup.alpha = y;
                y +=  animationSpeedMenuFadeIn/1000;
                yield return new WaitForSeconds(standardAnimationYieldTime);
            }
            
        }
        
        // - load a new level
        private void LoadLevel()
        {
            _inGame = true;
            loadingManager.StartScene(_levelName);
            
            // - regen of all the pools used in the game in precedent levels
            
            ObjectPoolingManager.Instance.RegenPools();
        }

        // - start the game selecting the current level
        public void StartGame()
        {
            scoreDisplay.SetLives(startingLives);
            LoadLevel();
        }
        
        // - reload the same level
        private void ReloadLevel()
        {
            loadingManager.RestartScene(_levelName);
            ObjectPoolingManager.Instance.RegenPools();
        }
        
        // - restart the game from the same level
        private void RestartGame()
        {
            ReloadLevel();
        }
        
        // - reset all the games variables
        private void ResetGame()
        {
            ObjectPoolingManager.Instance.ResetPools();
            ClearUI();
            _levelName = ChooseLevel(level);
            scoreDisplay.SetLives(startingLives);
            scoreDisplay.ResetScore();
            UnpauseGame();
            _inGame = false;
        }
        private void ClearUI()
        {
            scoreDisplay.Close();
        }

        // - start game over scene
        private void StartGameOver()
        {
            // - If we want to start from the beginning of the game
            
            StartCoroutine(StartGameOverCoroutine());
            level = 0;
            
            // - Background music set to Losing music
            
            audioManager.StopBackgroundMusic();
            audioManager.PlayGameOver();
        }

        IEnumerator StartGameOverCoroutine()
        {
            // - show the game over screen
            
            SceneManager.LoadScene("DeathScene");
            yield return null;
        }
        
        // - show the start screen menu
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

        private string ChooseLevel(int n)
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
            _endingTimeLevel = Time.time;
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

        public int GetStartingLives()
        {
            return startingLives;
        }

        public void TakeDamage()
        {
            scoreDisplay.SubToScore(damageLostPoints);
        }

        
        public void KillEnemy(int points, Vector3 positionEnemy, Quaternion rotation)
        {
            // - reward spawn
            SpawnCoins(positionEnemy, rotation);
            // - death sound of the enemy
            audioManager.PlayDieEnemie();
            scoreDisplay.AddToScore(points);
        }

        private void SpawnCoins(Vector3 position, Quaternion rotation)
        {
            Random random = new Random();
            int num = random.Next(3, 6);
            
            
            for (int i = 0; i < num; i++)
            {
                GameObject spawn = ObjectPoolingManager.Instance.GetObject (_coinPrefab.name);
                StartCoroutine(EntranceAnimationCoroutine(position, rotation, spawn));
                Component[] components = spawn.GetComponents(typeof(Component));
                foreach (Component component in components)
                {
                    if (component is Behaviour)
                    {
                        ((Behaviour)component).enabled = true;
                    }
                }
            }
            
        }
        IEnumerator EntranceAnimationCoroutine(Vector3 position, Quaternion rotation, GameObject obj)
        {
            bool notFinished = true;
            Random random = new Random();
            int x;
            int y;
            int sign = random.Next(-1, 1);
            int moveTime = random.Next(100, 200);
            int time = 0;
            
            float incrementx;
            float incrementy;
            float var = 0;
            obj.gameObject.transform.localScale = new Vector3(0, 0, 0);
            obj.gameObject.transform.position = position;
            
            // - if the enemy is upperside or not
            if (rotation.x == 0)
            {
                y = 1;
            }
            else
            {
                y = -1;
            }
            x = random.Next(7, 12);
            while (notFinished)
            {
                var += 0.20f;
                incrementx = var * sign / x;
                
                // - function for the bounce
                
                incrementy = (float) (y * (Math.Exp(-0.5f * var) * Math.Abs((x/3) * Math.Sin(var))));
                if (obj.gameObject.transform.localScale.x < 0.7f)
                {
                    obj.gameObject.transform.localScale += new Vector3(8f / moveTime, 8f / moveTime,8f / moveTime) ;
                }
                time++;
                obj.gameObject.transform.position = new Vector3(position.x + incrementx, position.y + incrementy, obj.gameObject.transform.position.z);
                if (time > moveTime){
                    notFinished = false;
                }
                yield return null;  
            }
        }
        public void KillPlayer()
        {
            SetIsPlayerDead(true);
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
                _endingTimeLevel = Time.time;
                StartGameOver();
            }
            SetIsPlayerDead(false);
        }

        public void SetInitialTimeLevel(float time)
        {
            _initialTimeLevel = time;
        }
        public float ActualTimeLevel()
        {
            return Time.time - _initialTimeLevel;
        }
        public float FinalTimeLevel()
        {
            return _endingTimeLevel - _initialTimeLevel;
        }

        private void PauseGame()
        {
            // commands to pause the 
            Time.timeScale = 0;
        }

        private void UnpauseGame()
        {
            // commands to restart the game from the pause point
            Time.timeScale = 1;
        }

        IEnumerator UpdateDuringGame()
        {
            while (true)
            {
                ScoreManager.instance.UpdateTime();
                if (_inGame)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (menuManager.GameMenuOpened())
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


