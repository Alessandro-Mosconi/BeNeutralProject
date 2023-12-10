using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using TMPro.Examples;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public float timerTime = 5;
    public float timeRemaining = 5;
    public bool timerIsRunning = false;
    
    private GameObject cameraObject;
    private CameraAnimator cameraAnimator;
    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
        timeRemaining = timerTime;
        cameraObject = GameObject.Find("Virtual Camera");
        cameraAnimator = cameraObject.GetComponent<CameraAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (cameraAnimator != null && cameraAnimator.arePlayerTooDistant)
        {
            timerIsRunning = true;
            UpdateTimer();
        }
        else
        {
            ResetTimer();
        }

        
    }

    private void UpdateTimer()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                print("kill-manager");
                GameManager.instance.KillPlayer();
                ResetTimer();
                gameObject.SetActive(false);
            }
        }
    }

    public void ResetTimer()
    {
        timerIsRunning = false;
        timeRemaining = timerTime;
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.SetText("Players are too distance\n Get closer in: \n" + string.Format("{0:00}:{1:00}", minutes, seconds));
    }
}
