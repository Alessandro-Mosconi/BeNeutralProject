using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLastPlayer : MonoBehaviour
{
    
    [SerializeField]
    public Transform player1;
    
    [SerializeField]
    public Transform player2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player1.transform.position.x > player2.transform.position.x)
        {
            transform.position = new Vector3(player1.transform.position.x, transform.position.y, -10);
        }
        else
        {
            transform.position = new Vector3(player2.transform.position.x, transform.position.y, -10);
        }
    }
}
