using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    public GameObject bulletPrefab;
    
    private Vector2 _shootingBaseDirection;
    [SerializeField] private Transform firingTransform;
    
    private PlayerMovement playerMovementScript;

    private int playerNumber = 1;
    
    private void Awake()
    {
        // create 50 lasers objects if it needs more it
        ObjectPoolingManager.Instance.CreatePool (bulletPrefab, 50, 100);
        
        GetComponents();
    }
    
    // Update is called once per frame
    void Update()
    {
        // fire
        if (playerMovementScript.playerNumber!=null && Input.GetButtonDown("FirePlayer" + playerMovementScript.playerNumber))
        {
            Fire();
        }
    }
    
    private void Fire()
    {
        GameObject go = ObjectPoolingManager.Instance.GetObject (bulletPrefab.name);
        go.transform.position = firingTransform.position;
        
        Vector3 shootingDirection = Vector3.up;
        if (playerMovementScript.movementDirection.x > 0)
        {
            shootingDirection = Vector3.up;
        } else if (playerMovementScript.movementDirection.x < 0)
        {
            shootingDirection = Vector3.down;
        }
         
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, shootingDirection);
        go.transform.rotation =  rotation;
        
    }
    
    private void GetComponents()
    {
        
        
        playerMovementScript = GetComponent<PlayerMovement>();

        ShootingPoint firingPositionComponent = GetComponentInChildren<ShootingPoint>();
        if (firingPositionComponent != null)
        {
            firingTransform = firingPositionComponent.gameObject.transform;
        }

    }
    
}
