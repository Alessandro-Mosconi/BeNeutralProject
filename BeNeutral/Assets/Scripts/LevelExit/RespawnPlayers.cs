using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private Vector3 player1Pos;
    private Vector3 player2Pos;
    void Start()
    {
      player1Pos = player1.transform.position;
      player2Pos = player2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(player1.activeSelf)|| !(player2.activeSelf))
        {
            respawnPlayers();
        }
    }
    
    
    public void respawnPlayers()
    {
            player1.transform.position = player1Pos;
            player2.transform.position = player2Pos;
            player1.SetActive(true);
            player2.SetActive(true);
        

    }
}
