using System.Collections;
using System.Collections.Generic;
using UI;
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


    private PlayerMovement p1Movement;
    private PlayerMovement p2Movement;

    
    
    // private SwitchPortalScript switch;
    void Start()
    {

        p1Movement = player1.GetComponent<PlayerMovement>();
        p2Movement = player2.GetComponent<PlayerMovement>();
        
        
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
            // pos1.y = player1.gameObject.GetComponent<Rigidbody2D>().gravityScale * checkpoint1.y;
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
            // - falling player audio
            respawnPlayers();
            AudioManager.instance.PlayFallPlayer();
            
        }
    }


    public void respawnPlayers()
    {


        if (p1Movement.gravityDirection<1 || p2Movement.gravityDirection>-1)
        {
            Vector2 support = player1.transform.position;
            player1.transform.position = player2.transform.position;
            player2.transform.position = support;
            
            p1Movement.gravityDirection = p1Movement.gravityDirection * -1;
            p2Movement.gravityDirection = p2Movement.gravityDirection * -1;

        }
        pSc1.transform.position = pos1;
        pSc2.transform.position = pos2;
        // yield return null;
        pSc1.Fell=false;
        // yield return null;
        pSc2.Fell=false;

        //Reset object pools to avoid having past bullets
        //TODO: Have a manager handle player respawn (so we can reset some state of the level if necessary)
        ObjectPoolingManager.Instance.ResetPools();
    }
}