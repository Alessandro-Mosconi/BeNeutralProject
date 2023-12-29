using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private HitPointsSO hitPoints;
    //hide in inspector
    [HideInInspector]
    public PlayerManager player;
    [SerializeField] private Image meterImage;
    [SerializeField] private Image staminaImage;
    private float maxHitPoints;
    private float maxStamina;

    private void Start()
    {

        maxHitPoints = player.MaxHitPoints;
        maxStamina = player.MaxStamina;

        // fixed the issue by moving the above line into the if statement
    }

    private void Update()
    {
        if (player != null)
        {
            // maxHitPoints = player.MaxHitPoints;
            meterImage.fillAmount = hitPoints.HitPointValue / maxHitPoints;
            staminaImage.fillAmount = hitPoints.StaminaValue/maxStamina;
        }
    }

    private float MaxHitPoints
    {
        get { return maxHitPoints; }
    }

}
