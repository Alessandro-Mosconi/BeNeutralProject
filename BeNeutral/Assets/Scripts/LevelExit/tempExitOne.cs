using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class tempExitOne : MonoBehaviour
{
    private bool playerOneExited = false;
    private PlayerTwoExit p2e;

    private void Start()
    {
        
        p2e = FindObjectOfType<PlayerTwoExit>();
    }

    public bool PlayerOneExited
    {
        get{return playerOneExited;}
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        playerOneExited = true;
        
        if (playerOneExited && p2e.PlayerTwoExited)
        {
            // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // SceneManager.LoadScene(currentSceneIndex + 1);
            GameManager.instance.ShowStartScreen();

        }
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        playerOneExited = false;
    }
}