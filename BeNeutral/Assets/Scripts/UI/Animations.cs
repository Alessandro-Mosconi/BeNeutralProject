using System.Collections;
using UnityEngine;

namespace UI
{
    public class Animations : Singleton<Animations>
    {
        public void FadeIn(CanvasGroup canvas, float duration)
        {
            StartCoroutine(FadeInCoroutine(canvas, duration));
        }

        IEnumerator FadeInCoroutine(CanvasGroup canvas, float duration)
        {
            if (canvas != null)
            {
                float step = 0.001f;
                float increment = (1f/duration)*step;
                float x = 0f;
                canvas.alpha = x;
                canvas.gameObject.SetActive(true);
                while (x < 1f && canvas != null)
                {
                    canvas.alpha = x;
                    x += increment;
                    yield return new WaitForSeconds(step);
                }
                canvas.alpha = 1;
            }
            
            yield return null;
        }
        
        public void FadeOut(CanvasGroup canvas, float duration)
        {
            StartCoroutine(FadeOutCoroutine(canvas, duration));
        }

        IEnumerator FadeOutCoroutine(CanvasGroup canvas, float duration)
        {
            float step = 0.001f;
            float decrement = (1f/duration)*step;
            float x = 1f;
            canvas.alpha = x;
            
            while (x > 0f)
            {
                canvas.alpha = x;
                x -= decrement;
                yield return new WaitForSeconds(step);
            }
            canvas.alpha = 0;
            canvas.gameObject.SetActive(false);
            yield return null;
        }
    }
}
