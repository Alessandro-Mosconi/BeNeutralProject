using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class SecretLevelRespawn : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private PlayerManager pSc1;
    private PlayerManager pSc2;
    private Vector3 pos1;
    private Vector3 pos2;
    public Vector3 checkpoint1;
    public Vector3 checkpoint2;
    // private Vector3 checkpoint3;
    public BoxCollider2D[] checkpoints;


    private Vector3 ch1p1y;
    private Vector3 ch1p2y;
    
    private Vector3 ch2p1y;
    private Vector3 ch2p2y;
    
    
    
    
    void Start()
    { 
        pSc1 = player1.GetComponent<PlayerManager>();
        pos1 = pSc1.transform.position;
        pSc2 = player2.GetComponent<PlayerManager>();
        pos2 = pSc2.transform.position;
        checkpoints = GetComponentsInChildren<BoxCollider2D>();
        checkpoint1 = checkpoints[0].transform.position;
        checkpoint2 = checkpoints[1].transform.position;
        // checkpoint3 = checkpoints[2].transform.position;

        ch1p1y = GameObject.Find("C1respawnYplayer1").GetComponent<Transform>().position;
        ch1p2y = GameObject.Find("C1respawnYplayer2").GetComponent<Transform>().position;
        
        ch2p1y = GameObject.Find("C2respawnYplayer1").GetComponent<Transform>().position;
        ch2p2y = GameObject.Find("C2respawnYplayer2").GetComponent<Transform>().position;
        
        
        
        
        

    }

    // Update is called once per frame
    void Update()
    {
        if (pSc1.IsSecretCheckpoint1 || pSc2.IsSecretCheckpoint1)
        {
            // print("secret level checkpoint 1");
            pos1 = checkpoint1;
            pos2 = checkpoint1;
            pos1.y = ch1p1y.y;
            pos2.y = ch1p2y.y;
        }
        
        if (pSc1.IsSecretCheckpoint2 || pSc2.IsSecretCheckpoint2)
        {
            pos1 = checkpoint2;
            pos2 = checkpoint2;
            pos1.y = ch2p1y.y;
            pos2.y = ch2p2y.y;
        }
        
        if (pSc1.Fell || pSc2.Fell)
        {
            // - falling player audio
            SrespawnPlayers();
            AudioManager.instance.PlayFallPlayer();
            
        }
    }


    public void SrespawnPlayers()
    {
        
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
