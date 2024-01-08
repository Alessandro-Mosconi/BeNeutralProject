using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UI;


public class Player0neExit : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator animator;
    
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
        
        other.gameObject.SetActive(false);
        animator.SetBool("player_entered", true);
        
        playerOneExited = true;
        if (playerOneExited && p2e.PlayerTwoExited)
        {
            // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // SceneManager.LoadScene(currentSceneIndex + 1);
            GameManager.instance.ShowNextLevel();

        }
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        playerOneExited = false;
    }
}
