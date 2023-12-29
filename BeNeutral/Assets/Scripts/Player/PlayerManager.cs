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

    //Stamina
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaConsumption;
    private MagneticField magneticField;
    
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
     private bool isCheckpoint2;
     private bool isCheckpoint3;
     private bool fell;
     private void Start()
     {
         magneticField = GetComponent<MagneticField>();
         hitPoints.HitPointValue = startingHitPoints;
         hitPoints.StaminaValue = maxStamina;
         healthBar = Instantiate(healthBarPrefab);
         healthBar.player = this;
         //
         

     }

     public float MaxHitPoints
     {
         get { return maxHitPoints; }
     }
     public float MaxStamina
     {
         get { return maxStamina; }
     }

     private void Update()
     {
         fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
         if (magneticField.isActive)
         {
             ConsumeStamina();
         }
         else
         {
             RegenerateStamina();
         }
         
     }

     private void OnTriggerEnter2D(Collider2D other)
     {
         string damageType = other.tag;
         if (other.CompareTag("FallDetector") || other.CompareTag("Hazards"))
         {
             DamagePlayer(damageType == "FallDetector" ? fallDamageValue : hazardDamageValue);
             fell = true;
         } else if (other.CompareTag("checkpoint"))
         {
             isCheckpoint = true;
         }else if (other.CompareTag("checkpoint2"))
         {
             isCheckpoint2 = true;
             isCheckpoint = false;
         } else if (other.CompareTag("checkpoint3"))
         {
             isCheckpoint3 = true;
             isCheckpoint2 = false;
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
     public bool IsCheckpoint2
     {
         get { return isCheckpoint2; }

     }

     public bool IsCheckpoint3
     {
         get { return isCheckpoint3; }
     }

     public bool Fell
     {
         get { return fell; }
         set { fell = value; }

     }

     private void ConsumeStamina()
     {
         if (hitPoints.StaminaValue !=0)
         {
             hitPoints.StaminaValue -= staminaConsumption * Time.deltaTime;
         }
         if (hitPoints.StaminaValue <= float.Epsilon)
         {
             magneticField.DisattivaMagneticField();
             hitPoints.StaminaValue = 0;
         }
     }
     private void RegenerateStamina()
     {
         hitPoints.StaminaValue += staminaConsumption * Time.deltaTime;
         if (hitPoints.StaminaValue > maxStamina)
         {
             hitPoints.StaminaValue = maxStamina;
         }
     }

     
     
}
