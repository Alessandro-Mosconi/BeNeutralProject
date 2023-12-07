using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private HitPointsSO hitPoints;
    [SerializeField] private float maxHitPoints;
    [SerializeField] private float startingHitPoints;

     public HealthBar healthBarPrefab;
     private HealthBar healthBar;
     
     //fall detection
     private Vector3 respawnPoint;
     [SerializeField] private GameObject fallDetector;

     private void Start()
     {
         hitPoints.HitPointValue = startingHitPoints;
         healthBar = Instantiate(healthBarPrefab);
         healthBar.player = this;
         //
         respawnPoint = transform.position;
     }

     public float MaxHitPoints
     {
         get { return maxHitPoints; }
     }

     private void Update()
     {
         fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
         
     }

     private void OnTriggerEnter2D(Collider2D other)
     {
         
         if (other.CompareTag("FallDetector"))
         {
             DamagePlayer();
         }
     }

     private void KillPlayer()
     {
         // Destroy(GameObject);
     }

     private void ResetPlayer()
     {
         
     }

     private void DamagePlayer()
     {
         // Debug.Log("succes tag compare");
         hitPoints.HitPointValue = hitPoints.HitPointValue - 0.5f;
         transform.position = respawnPoint;
     }
}
