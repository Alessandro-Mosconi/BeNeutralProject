using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;

public class FollowLastPlayer : MonoBehaviour
{
    
    [SerializeField] [NotNull] public Transform player1;
    [SerializeField] [NotNull] public Transform player2;
    [SerializeField] public float verticalOffset = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Mathf.Max(player1.position.x, player2.position.x),
            ((player1.position.y + player2.position.y) * 0.5f) + verticalOffset, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 p1Pos = player1.position;
        Vector3 p2Pos = player2.position;
        transform.position = new Vector3(Mathf.Max(p1Pos.x, p2Pos.x), ((p1Pos.y + p2Pos.y) * 0.5f) + verticalOffset, transform.position.z);
    }
}
