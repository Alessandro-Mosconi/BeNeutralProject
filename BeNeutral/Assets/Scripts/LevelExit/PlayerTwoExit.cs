using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UI;


public class PlayerTwoExit : MonoBehaviour
{
    
    [SerializeField] private Animator animator;
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
        
        other.gameObject.SetActive(false);
        animator.SetBool("player_entered", true);
        
        playerTwoExited = true;
        
        if (playerTwoExited && p1e.PlayerOneExited)
        {
            // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // SceneManager.LoadScene(currentSceneIndex + 1);
            GameManager.instance.ShowNextLevel();

        }
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        playerTwoExited = false;
    }
}
