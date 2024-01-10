using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject alertScreen;
    [SerializeField] private Timer timerScript;

    public bool arePlayerTooDistant { get; private set; }
    
    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        alertScreen = GameObject.Find("AlertScreen");
        timerScript = GameObject.Find("Timer").GetComponent<Timer>();
        
        alertScreen.SetActive(false);
        animator = GetComponent<Animator>();
        arePlayerTooDistant = false;


    }

    void Update()
    {
        arePlayerTooDistant = Math.Abs(player1.transform.position.x - player2.transform.position.x) > 20f;
        if (arePlayerTooDistant)
        {
            alertScreen.SetActive(true);
            animator.SetBool("isShaking", true);
        }
        else
        {
            timerScript.ResetTimer();
            alertScreen.SetActive(false);
            animator.SetBool("isShaking", false);
        }
    }
}