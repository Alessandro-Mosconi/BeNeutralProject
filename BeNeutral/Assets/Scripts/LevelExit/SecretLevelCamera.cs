using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;

public class SecretLevelCamera : MonoBehaviour
{
    
    [SerializeField] [NotNull] public Transform Entrance1;
    [SerializeField] [NotNull] public Transform Entrance2;

    [SerializeField] private Transform Player1;
    [SerializeField] private Transform Player2;

    // [SerializeField] public float verticalOffset = 0;
    // [SerializeField] public float maxPlayerDistance = 20f;
    public CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera _vcam;


    private Vector3 p1Pos;
    private Vector3 p2Pos;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Mathf.Max(Entrance1.position.x, Entrance2.position.x),
            ((Entrance1.position.y + Entrance2.position.y) * 0.5f) , transform.position.z);
        if (_vcam == null)
        {
            _vcam = (cinemachineBrain == null) ? null : cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         p1Pos = Player1.position;
         p2Pos = Player2.position;
        float middleX = (p1Pos.x + p2Pos.x) * 0.5f;
        //Mathf.Max(p1Pos.x, p2Pos.x)
        transform.position = new Vector3(middleX +2.5f, ((p1Pos.y + p2Pos.y) * 0.5f) , transform.position.z);

        // if (Math.Abs(p1Pos.y - p2Pos.y) > maxPlayerDistance)
        // {
            //vcam.m_Lens.OrthographicSize = p1Pos.y - p2Pos.y;
            if (_vcam == null)
            {
                print(Math.Min( Math.Abs(p1Pos.y - p2Pos.y), 30f));
                _vcam = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
            }
            else
            {
                // _vcam.m_Lens.OrthographicSize = Math.Min( Math.Abs(p1Pos.y - p2Pos.y), 20f);
                // _vcam.m_Lens.OrthographicSize = Math.Min( Math.Abs(p1Pos.y - p2Pos.y), 30f);

            }
        // }
        //
        // else
        // {
        //     if (_vcam == null)
        //     {
        //         _vcam = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        //     }
        //     else
        //     {
        //         _vcam.m_Lens.OrthographicSize = 20f;
        //     }
        // }
    }
}
