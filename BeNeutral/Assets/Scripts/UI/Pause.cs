using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Sprite pauseImg;
    [SerializeField] private Sprite unpauseImg;
    private bool isInPause = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPauseButtonPress()
    {
        if (isInPause)
        {
            UnPauseGame();
            isInPause = false;
        }
        else
        {
            PauseGame();
            isInPause = true;
        }
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
        GetComponent<Image>().sprite = unpauseImg;
        print("pause");
        //non va
        //GameManager.instance.PauseGame();
    }
    private void UnPauseGame()
    {
        GetComponent<Image>().sprite = pauseImg;
        Time.timeScale = 1;
        print("pause");
        //non va
        //GameManager.instance.PauseGame();
    }
}
