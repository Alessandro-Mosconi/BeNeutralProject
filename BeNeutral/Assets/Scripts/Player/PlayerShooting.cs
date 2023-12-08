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
    
    private void Awake()
    {
        ObjectPoolingManager.Instance.CreatePool (bulletPrefab, 1000, 2000);
        
        GetComponents();
    }
    
    // Update is called once per frame
    void Update()
    {
        // fire
        if (Input.GetButtonDown("FirePlayer" + playerMovementScript.playerNumber))
        {
            Fire();
        }
    }
    
    private void Fire()
    {
        GameObject go = ObjectPoolingManager.Instance.GetObject (bulletPrefab.name);
        go.transform.SetParent(firingTransform);
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
