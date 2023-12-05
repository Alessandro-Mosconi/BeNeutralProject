using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerTwoExit : MonoBehaviour
{
    private bool playerTwoExited = false;
    private Player0neExit p1e;

    private void Start()
    {
        p1e = FindObjectOfType<Player0neExit>();
    }

    public bool PlayerTwoExited
    {
        get{return playerTwoExited;}
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        playerTwoExited = true;
        
        if (playerTwoExited && p1e.PlayerOneExited)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex - 1);
        }
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        playerTwoExited = false;
    }
}
