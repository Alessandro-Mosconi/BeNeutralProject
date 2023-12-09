using System;
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
    [SerializeField] public float maxPlayerDistance = 6f;
    private CinemachineVirtualCamera vcam;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Mathf.Max(player1.position.x, player2.position.x),
            ((player1.position.y + player2.position.y) * 0.5f) + verticalOffset, transform.position.z);
        
        var camera = Camera.main;
        var brain = (camera == null) ? null : camera.GetComponent<CinemachineBrain>();
        vcam = (brain == null) ? null : brain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 p1Pos = player1.position;
        Vector3 p2Pos = player2.position;
        float middleX = (p1Pos.x + p2Pos.x) * 0.5f;
        //Mathf.Max(p1Pos.x, p2Pos.x)
        transform.position = new Vector3(middleX, ((p1Pos.y + p2Pos.y) * 0.5f) + verticalOffset, transform.position.z);

        if (Math.Abs(p1Pos.y - p2Pos.y) > maxPlayerDistance)
        {
            //vcam.m_Lens.OrthographicSize = p1Pos.y - p2Pos.y;
            vcam.m_Lens.OrthographicSize = Math.Min( p1Pos.y - p2Pos.y, 9f);
        }
        
        else
        {
            vcam.m_Lens.OrthographicSize = 6f;
        }
    }
}
