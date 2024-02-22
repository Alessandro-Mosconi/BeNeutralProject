using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class SwitchPlayer : MonoBehaviour
{
    
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;
    
    public void switchPlayer()
    {
        Vector2 support = Player1.transform.position;
        Player1.transform.position = Player2.transform.position;
        Player2.transform.position = support;

        PlayerMovement pl1Movement = Player1.GetComponent<PlayerMovement>();
        pl1Movement.gravityDirection = pl1Movement.gravityDirection * -1;
        PlayerMovement pl2Movement = Player2.GetComponent<PlayerMovement>();
        pl2Movement.gravityDirection = pl2Movement.gravityDirection * -1;
    }
}
