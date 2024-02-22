using UnityEngine;

namespace UI
{
    
    public class StartButton : MonoBehaviour
    {
            [Header("EASY")]
            [SerializeField] private int initialLivesE;
            [SerializeField] private int levelPointE;
            [SerializeField] private int damageLostPointE;
            [SerializeField] private int dieLostPointE;

            [SerializeField] private float maxHitPointsE,
                startingHitPointsE,
                fallDamageValueE,
                hazardDamageValueE,
                continuousDamageValueE,
                playerDamageE;
            
            [Header("MEDIUM")]
            [SerializeField] private int initialLivesM;
            [SerializeField] private int levelPointM;
            [SerializeField] private int damageLostPointM;
            [SerializeField] private int dieLostPointM;
            [SerializeField] private float maxHitPointsM,
                startingHitPointsM,
                fallDamageValueM,
                hazardDamageValueM,
                continuousDamageValueM,
                playerDamageM;
            
            [Header("HARD")]
            [SerializeField] private int initialLivesH;
            [SerializeField] private int levelPointH;
            [SerializeField] private int damageLostPointH;
            [SerializeField] private int dieLostPointH;
            [SerializeField] private float maxHitPointsH,
                startingHitPointsH,
                fallDamageValueH,
                hazardDamageValueH,
                continuousDamageValueH,
                playerDamageH;  
            
            
            public void Easy()
            {
                GameManager.instance.SetParameters(initialLivesE, levelPointE, damageLostPointE, dieLostPointE, maxHitPointsE, startingHitPointsE, fallDamageValueE, hazardDamageValueE, continuousDamageValueE, playerDamageE);
                GameManager.instance.StartGame();
            }
            
            public void Medium()
            {
                GameManager.instance.SetParameters(initialLivesM, levelPointM, damageLostPointM, dieLostPointM, maxHitPointsM, startingHitPointsM, fallDamageValueM, hazardDamageValueM, continuousDamageValueM, playerDamageM);
                GameManager.instance.StartGame();
            }
            
            public void Hard()
            {
                GameManager.instance.SetParameters(initialLivesH, levelPointH, damageLostPointH, dieLostPointH, maxHitPointsH, startingHitPointsH, fallDamageValueH, hazardDamageValueH, continuousDamageValueH, playerDamageH);
                GameManager.instance.StartGame();
            }
    
    }
}
