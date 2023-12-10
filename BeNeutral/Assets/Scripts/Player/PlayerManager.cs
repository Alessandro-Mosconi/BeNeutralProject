using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private HitPointsSO hitPoints;
    [SerializeField] private float maxHitPoints;
    [SerializeField] private float startingHitPoints;
    
     public HealthBar healthBarPrefab;
     private HealthBar healthBar;
     
     //Damage value
     private float fallDamageValue = 3f;
     private float hazardDamageValue = 1f;
     
     //fall detection
     // private Vector3 playerPos;
     [SerializeField] private GameObject fallDetector;

     
     //
     private bool isCheckpoint;
     private bool fell;
     private void Start()
     {
         hitPoints.HitPointValue = startingHitPoints;
         healthBar = Instantiate(healthBarPrefab);
         healthBar.player = this;
         //
         
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
         string damageType = other.tag;
         if (other.CompareTag("FallDetector") || other.CompareTag("Hazards"))
         {
             DamagePlayer(damageType == "FallDetector" ? fallDamageValue : hazardDamageValue);
         } else if (other.CompareTag("checkpoint"))
         {
             isCheckpoint = true;
         }
     }

     private void KillPlayer()
     {
             // Destroy(gameObject);
             // Destroy(healthBar.gameObject);
         
         
     }

     private void ResetPlayer()
     {
         hitPoints.HitPointValue = startingHitPoints;
         healthBar = Instantiate(healthBarPrefab);
         healthBar.player = this;
     }

     public void DamagePlayer(float damage)
     {
         GameManager.instance.TakeDamage();
         hitPoints.HitPointValue -= damage;
         
         if (hitPoints.HitPointValue <= float.Epsilon)
         {
             KillPlayer();
             ResetPlayer();
             
             // Game manager lives -1
             // Game manager restart current level
             GameManager.instance.KillPlayer();
         }
     }

     
     public bool IsCheckpoint
     {
         get { return isCheckpoint; }
         
     }
     
     public bool Fell
     {
         get { return fell; }
         set { fell = value; }

     }

     
     
}
