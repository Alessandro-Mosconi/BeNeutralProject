using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPoint : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerMovement playerMovementScript;
    private float distFromPlayer = 0.4f;
    
    private void Awake()
    {
        GetComponents();
    }
    
    void Update()
    {
        Vector3 transformPosition = transform.position;
        
        
        if (playerMovementScript!=null && playerMovementScript.movementDirection.x > 0)
        {
            transform.position = new Vector2((playerTransform.position.x + distFromPlayer), transformPosition.y);
        } else if (playerMovementScript!=null && playerMovementScript.movementDirection.x < 0)
        {
            transform.position = new Vector2((playerTransform.position.x - distFromPlayer), transformPosition.y);
        }
    }
    
    private void GetComponents()
    {
        
        playerMovementScript = GetComponentInParent<PlayerMovement>();
        if (playerMovementScript == null)
        {
            print("ecco");
        }

        playerTransform = transform.parent;
        
        distFromPlayer = Math.Abs(transform.position.x - playerTransform.position.x);

    }
}
