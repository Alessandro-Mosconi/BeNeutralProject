using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private ScoreDisplay scoreDisplay;
    [SerializeField] private StartGame startGame;
   // [SerializeField] private GameOverScreen gameOverScreen;
   

    private int lifes = 3;
    private int level = 0;
    
    
    public void start()
    {
        //TODO
        startGame.LevelName = "Level1";
        // start background music
        // start animations on the load screen
    }

    public void StartGame()
    {
        print("Game start!");
        // get starting level

        startGame.loadLevel();
        
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

        // update the ui
       // scoreDisplay.UpdateLifes(3);
    }

    private void ClearUI()
    {
       //   scoreDisplay.Close();
   //     gameOverScreen.gameObject.SetActive(false);
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
        // - disable the HUD
        // - show the game over screen
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

    private void UpdateUI()
    {
       // scoreDisplay.UpdateLifes(lifes);
    }

    public void ShowStartScreen()
    {
        // TODO
        // - clear all the UI
        // - activate the start screen
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
         //   UpdateUI();
    }
    
}

