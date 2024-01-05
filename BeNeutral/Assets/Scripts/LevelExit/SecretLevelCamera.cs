using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;

public class SecretLevelCamera : MonoBehaviour
{
    
    [SerializeField] [NotNull] public Transform EntrancePlayer1;
    [SerializeField] [NotNull] public Transform EntrancePlayer2;
    [SerializeField] public float verticalOffset = 0;
    [SerializeField] public float maxPlayerDistance = 6f;
    public CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera _vcam;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Mathf.Max(EntrancePlayer1.position.x, EntrancePlayer1.position.x),
            ((EntrancePlayer1.position.y + EntrancePlayer1.position.y) * 0.5f) + verticalOffset, transform.position.z);
        if (_vcam == null)
        {
            _vcam = (cinemachineBrain == null) ? null : cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 p1Pos = EntrancePlayer1.position;
        Vector3 p2Pos = EntrancePlayer1.position;
        float middleX = (p1Pos.x + p2Pos.x) * 0.5f;
        //Mathf.Max(p1Pos.x, p2Pos.x)
        transform.position = new Vector3(middleX +2.5f, ((p1Pos.y + p2Pos.y) * 0.5f) + verticalOffset, transform.position.z);

        // if (Math.Abs(p1Pos.y - p2Pos.y) > maxPlayerDistance)
        // {
            //vcam.m_Lens.OrthographicSize = p1Pos.y - p2Pos.y;
            if (_vcam == null)
            {
                print(Math.Min( Math.Abs(p1Pos.y - p2Pos.y), 20f));
                _vcam = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
            }
            else
            {
                // _vcam.m_Lens.OrthographicSize = Math.Min( Math.Abs(p1Pos.y - p2Pos.y), 20f);
                _vcam.m_Lens.OrthographicSize = Math.Min( Math.Abs(p1Pos.y - p2Pos.y), 20f);

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
