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
    private Vector3 colliderPosition;
    private Vector3 colliderDimensions;
    void Start()
    { 
        pSc1 = player1.GetComponent<PlayerManager>();
        pos1 = pSc1.transform.position;
        pSc2 = player2.GetComponent<PlayerManager>();
        pos2 = pSc2.transform.position;
        // colliderDimensions = GetComponentInChildren<BoxCollider2D>().offset;
        colliderPosition = GetComponentInChildren<BoxCollider2D>().transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (pSc1.IsCheckpoint || pSc2.IsCheckpoint)
        {
            
            pos1 = colliderPosition;
            pos2 = colliderPosition;
            pos2.y = -colliderPosition.y;
            

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