using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEditor.Build.Content;
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
     // private Vector3 respawnPoint;
     [SerializeField] private GameObject fallDetector;

     private void Start()
     {
         hitPoints.HitPointValue = startingHitPoints;
         healthBar = Instantiate(healthBarPrefab);
         healthBar.player = this;
         //
         // respawnPoint = transform.position;
         
         //
         Debug.Log("OBJECT TYPE: "+gameObject.GetType());
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
             DamagePlayer(damageType);
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
         // respawnPoint = transform.position;

         
     }

     private void DamagePlayer(string damageType)
     {
         GameManager.instance.TakeDamage();
         if (damageType == "FallDetector")
         {
             hitPoints.HitPointValue = hitPoints.HitPointValue - fallDamageValue;
             gameObject.SetActive(false);
             
         }else if (damageType == "Hazards")
         {
             hitPoints.HitPointValue = hitPoints.HitPointValue - hazardDamageValue;
             gameObject.SetActive(false);
         }
        
         if (hitPoints.HitPointValue <= float.Epsilon)
         {
             KillPlayer();
             ResetPlayer();
             
             // Game manager lives -1
             // Game manager restart current level
             GameManager.instance.KillPlayer();
             
             
         }
         


     }
     
}
