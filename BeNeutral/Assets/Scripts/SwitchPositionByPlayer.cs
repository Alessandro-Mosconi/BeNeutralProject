using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPositionByPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.position = new Vector3(child.position.x,
                GetPositionScale() * 4.5f, child.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChildrenPosition();
    }

    void UpdateChildrenPosition()
    {

        float positionScale = GetPositionScale();
        
        if ((player.transform.parent == null || player.transform.parent.gameObject.layer == LayerMask.NameToLayer("Terrain")) && positionScale * transform.GetChild(0).position.y < 0)
        {
            foreach (Transform child in transform)
            {
                    child.position = new Vector3(child.position.x,
                        positionScale * 4.5f, child.position.z);
            }
        }
        
    }

    float GetPositionScale()
    {
        return player.transform.localRotation.x == 0 ? 1 : -1;

    }
}
