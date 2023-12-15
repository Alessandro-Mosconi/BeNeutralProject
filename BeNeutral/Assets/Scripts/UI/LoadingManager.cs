using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoadingManager : Singleton<LoadingManager>
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private CanvasGroup loadingScreenGroup;
        
        [Header("Animations")]
        [SerializeField] private float timeFadeOut;
        [SerializeField] private float timeFadeIn;
        
        [SerializeField] private Image loadBar;

        // Update is called once per frame
        void Update()
        {
            
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void StartScene(string sceneName)
        {
            loadingScreen.SetActive(true);
            StartCoroutine(LoadScene(sceneName));
        }

        public void RestartScene(string sceneName)
        {
            ScoreManager.instance.Open();
            SceneManager.LoadScene(sceneName);
            AudioManager.Instance.ChooseBackgroundMusic(1);
        }

        IEnumerator LoadScene(string sceneName)
        {
            loadBar.fillAmount = 0;
            StartCoroutine(fadeInLoadingScreen(timeFadeIn));
            yield return new WaitForSeconds(timeFadeIn);
            ScoreManager.instance.Open();
            AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
            while (!loading.isDone)
            {
                loadBar.fillAmount = loading.progress;
                yield return null;
            }
            loadBar.fillAmount = 1;
            yield return new WaitForSeconds(1f);
            StartCoroutine(fadeOutLoadingScreen(timeFadeOut));
            AudioManager.Instance.ChooseBackgroundMusic(1);
        }

        IEnumerator fadeOutLoadingScreen(float time)
        {
            float timePassed = 0f;
            loadingScreenGroup.alpha = 1;
            while (timePassed < time)
            {
                loadingScreenGroup.alpha = 1 * (time - timePassed) / time;
                timePassed += Time.deltaTime;
                yield return null;
            }
            loadingScreenGroup.alpha = 0;
            loadingScreen.SetActive(false);
        }
        IEnumerator fadeInLoadingScreen(float time)
        {
            float timePassed = 0f;
            loadingScreenGroup.alpha = 0;
            while (timePassed < time)
            {
                loadingScreenGroup.alpha = 1 * (timePassed) / time;
                timePassed += Time.deltaTime;
                yield return null;
            }
            loadingScreen.SetActive(true);
            loadingScreenGroup.alpha = 1;
        }
    }
}
