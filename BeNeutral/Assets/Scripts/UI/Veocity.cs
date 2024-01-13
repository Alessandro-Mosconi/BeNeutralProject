using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Veocity : MonoBehaviour
{
    [SerializeField] private float velocityIncrementer = 2f;
    
    [SerializeField] private Sprite speedImg;
    [SerializeField] private Sprite normalImg;
    private bool isFast = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void IncrementButtonPressed()
    { 
        if (isFast)
        {
            Time.timeScale = 1;
            GetComponent<Image>().sprite = speedImg;
            isFast = false;
        }
        else
        {
            Time.timeScale = velocityIncrementer;
            GetComponent<Image>().sprite = normalImg;
            isFast = true;
        }
    }
}
