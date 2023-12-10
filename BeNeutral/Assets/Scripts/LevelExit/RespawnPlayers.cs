using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private PlayerManager pSc1;
    private PlayerManager pSc2;
    private Vector3 pos1;
    private Vector3 pos2;
    private Vector3 checkpoint1;
    private Vector3 checkpoint2;
    private Vector3 checkpoint3;
    private BoxCollider2D[] checkpoints;
    void Start()
    { 
        pSc1 = player1.GetComponent<PlayerManager>();
        pos1 = pSc1.transform.position;
        pSc2 = player2.GetComponent<PlayerManager>();
        pos2 = pSc2.transform.position;
        checkpoints = GetComponentsInChildren<BoxCollider2D>();
        checkpoint1 = checkpoints[0].transform.position;
        if (checkpoints.Length > 1)
        {
            checkpoint2 = checkpoints[1].transform.position;
            checkpoint3 = checkpoints[2].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pSc1.IsCheckpoint || pSc2.IsCheckpoint)
        {
            
            pos1 = checkpoint1;
            pos2 = checkpoint1;
            pos2.y = -checkpoint1.y;
        }
        
        if (pSc1.IsCheckpoint2 || pSc2.IsCheckpoint2)
        {
            pos1 = checkpoint2;
            pos2 = checkpoint2;
            pos2.y = -checkpoint2.y;
        }
        
        if (pSc1.IsCheckpoint3 || pSc2.IsCheckpoint3)
        {
            pos1 = checkpoint3;
            pos2 = checkpoint3;
            pos2.y = -checkpoint3.y;
        }
        if (pSc1.Fell || pSc2.Fell)
        {
            respawnPlayers();
        }
    }


    public void respawnPlayers()
    {
        
        pSc1.transform.position = pos1;
        pSc2.transform.position = pos2;
        pSc1.Fell=false;
        pSc2.Fell=false;


    }
}