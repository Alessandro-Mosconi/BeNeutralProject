using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoadingManager : Singleton<LoadingManager>
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private CanvasGroup loadingScreenGroup;
        [SerializeField] private TMP_Text adviceText;
        
        [Header("Animations")]
        [SerializeField] private float timeFadeOut;
        [SerializeField] private float timeFadeIn;
        
        [SerializeField] private Image loadBar;

        // - advices for the loading screen
        private List<string> _advices = new List<string>();

        public string GetRandomAdvice()
        {
            string advice;
            int len = _advices.Count;
            int n = RandomNumberGenerator.GetInt32(0, len - 1);
            advice = _advices[n];
            return advice;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void StartScene(string sceneName)
        {
            loadingScreen.SetActive(true);
            _advices.Add("No challenge is insurmountable when we work together. The key is collaboration and mutual trust.");
            _advices.Add( "In NeutralVille your team's strength is your most precious resource. Build your success together!\"");
            _advices.Add( "Communication is fundamental. Share ideas, strategies, and information to create a winning synergy");
            _advices.Add( "Think together, think outside the box. The brightest solutions emerge when we work as a collective mind.");
            _advices.Add( "Success is not a destination but a journey. Every step taken together brings you closer to the final victory.");
            _advices.Add( "In the team, every victory is a victory for all. Celebrate successes together and learn from the challenges faced.");

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
            Animations.instance.TypeWriterText(GetRandomAdvice(), adviceText, 15f, true);
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
            while (!Animations.instance._typeWriterEnded)
            {
                yield return null;
            }
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
