using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lifeDisplay;

    
    public void UpdateLifes(int lifes)
    {
        lifeDisplay.text = string.Format("{0:D6}", lifes);
    }
    
    
    
    public void Open()
    {
        gameObject.SetActive(true);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }

}