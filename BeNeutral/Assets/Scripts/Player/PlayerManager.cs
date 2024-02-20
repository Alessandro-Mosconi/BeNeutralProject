using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UI;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private HitPointsSO hitPoints;
    [SerializeField] private float maxHitPoints;
    [SerializeField] private float startingHitPoints;
    [SerializeField] private CoinManager coinManager;

    //Stamina
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaConsumption;
    private bool _slowerStaminaRecharge = false;
    private MagneticField magneticField;
    
     public HealthBar healthBarPrefab;
     private HealthBar healthBar;
     
     //Damage value
     private float fallDamageValue = 3f;
     private float hazardDamageValue = 1f;

     private float continuousDamageValue = 2.5f;
     private Coroutine damageCoroutine;
     
     //fall detection
     // private Vector3 playerPos;
     [SerializeField] public GameObject fallDetector;
     
     //
     private bool isCheckpoint;
     private bool isCheckpoint2;
     private bool isCheckpoint3;
     private bool fell;

     private bool isSecretCheckpoint1;
     private bool isSecretCheckpoint2;
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

     public MagneticField Field
     {
         get { return magneticField; }
     }

     public float HazardDamage
     {
         get { return hazardDamageValue; }
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
         }else if (other.CompareTag("SecretLevelCheckpoint1"))
         {
             isSecretCheckpoint1 = true;
             isCheckpoint = false;
         }else if (other.CompareTag("SecretLevelCheckpoint2"))
         {
             isSecretCheckpoint2 = true;
             isSecretCheckpoint1 = false;
         }
         else if (other.CompareTag("checkpoint2"))
         {
             isCheckpoint2 = true;
             isSecretCheckpoint2 = false;
         } else if (other.CompareTag("checkpoint3"))
         {
             isCheckpoint3 = true;
             isCheckpoint2 = false;
         } else if (other.CompareTag("Collectible"))
         {
             coinManager.CollectedCoins += 1;
             ScoreManager.instance.SetLives(ScoreManager.instance.GetLives() + coinManager.CreditLife());
         }
         
     }

     private void OnCollisionEnter2D(Collision2D other)
     {

         if (other.gameObject.CompareTag("RotatingPlatform"))
         {
             transform.parent = other.transform;
         }
         if (other.gameObject.CompareTag("EMP"))
         {
             if (damageCoroutine == null)
             {
                 damageCoroutine=StartCoroutine(ContinuousDamagePlayer(continuousDamageValue,1f));
             }

         }
     }

     private void OnCollisionExit2D(Collision2D other)
     {
         if (other.gameObject.CompareTag("RotatingPlatform"))
         {
             transform.parent = null;
         }
         if (other.gameObject.CompareTag("EMP"))
         {
             if (damageCoroutine != null)
             {
                 StopCoroutine(damageCoroutine);
                 damageCoroutine=null;
             }

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
             // KillPlayer();
             // ResetPlayer();
             
             // Game manager lives -1
             // Game manager restart current level
             GameManager.instance.KillPlayer();
         }
     }

     public IEnumerator ContinuousDamagePlayer(float damage, float interval)
     {
         while (true)
         {
             hitPoints.HitPointValue -= damage;

             if (hitPoints.HitPointValue <= float.Epsilon)
             {
                 GameManager.instance.KillPlayer();
                 break;
             }
             if (interval > float.Epsilon)
             {
                 yield return new WaitForSeconds(interval);
             }
             else
             {
                 break;
             }
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

     public bool IsSecretCheckpoint1
     {
         get { return isSecretCheckpoint1; }
     }
     public bool IsSecretCheckpoint2
     {
         get { return isSecretCheckpoint2; }
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
             _slowerStaminaRecharge = true;
         }
     }
     private void RegenerateStamina()
     {
         hitPoints.StaminaValue += staminaConsumption * (_slowerStaminaRecharge ? 0.3f : 1) * Time.deltaTime;
         if (hitPoints.StaminaValue > maxStamina)
         {
             hitPoints.StaminaValue = maxStamina;
             _slowerStaminaRecharge = false;
         }
     }

     public void StopMovementUntilKeyup()
     {
        GetComponent<PlayerMovement>().StopMovementUntilKeyup();
     }
}
