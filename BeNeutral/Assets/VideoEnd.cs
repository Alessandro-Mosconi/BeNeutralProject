using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Video;

public class VideoEnd : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;

    private bool isSceneChanged = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSceneChanged && (_videoPlayer.frame +1) == (long)_videoPlayer.frameCount)
        {
            isSceneChanged = true;
            Time.timeScale = 1;
            GameManager.instance.NextLevel();
        }
        
    }
}
