using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarTwo : MonoBehaviour
{
    [SerializeField] private HitPointsSO hitPoints;
    //hide in inspector
    [HideInInspector]
    public PlayerManager player;
    [SerializeField] private Image meterImage;
    private float maxHitPoints;

    private void Start()
    {

        maxHitPoints = player.MaxHitPoints;

        // fixed the issue by moving the above line into the if statement
    }

    private void Update()
    {
        if (player != null)
        {
            // maxHitPoints = player.MaxHitPoints;
            Debug.Log(player);
            meterImage.fillAmount = hitPoints.HitPointValue / maxHitPoints;
        }
    }

}