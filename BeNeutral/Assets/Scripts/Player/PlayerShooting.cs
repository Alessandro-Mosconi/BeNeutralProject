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
    
    
    private void Awake()
    {
        // create 50 lasers objects if it needs more it
        ObjectPoolingManager.Instance.CreatePool (bulletPrefab, 50, 50);
        
        GetComponents();
    }
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // fire
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }
    
    private void Fire()
    {
        // Laser laser = Instantiate(laserPrefab, firingTransform.position, firingTransform.rotation);
        GameObject go = ObjectPoolingManager.Instance.GetObject (bulletPrefab.name);
        go.transform.position = firingTransform.position;
        go.transform.rotation = firingTransform.rotation;
        
    }
    
    private void GetComponents()
    {

        ShootingPoint firingPositionComponent = GetComponentInChildren<ShootingPoint>();
        if (firingPositionComponent != null)
        {
            firingTransform = firingPositionComponent.gameObject.transform;
        }

    }
}
