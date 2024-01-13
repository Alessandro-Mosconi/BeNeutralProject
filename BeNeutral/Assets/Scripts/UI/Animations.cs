using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace UI
{
    public class Animations : Singleton<Animations>
    {
        public bool _typeWriterEnded;
        public void FadeIn(CanvasGroup canvas, float duration)
        {
            StartCoroutine(FadeInCoroutine(canvas, duration));
        }

        IEnumerator FadeInCoroutine(CanvasGroup canvas, float duration)
        {
            if (canvas != null)
            {
                float step = 0.001f;
                float increment = (10f/duration)*step;
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
            float decrement = (10f/duration)*step;
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

        public void GrowingTextAnimation(string text, TMP_Text place, int finalSizePixel)
        {
            StartCoroutine(GrowingTextCoroutine(text, place, finalSizePixel));
        }

        IEnumerator GrowingTextCoroutine(string text, TMP_Text place, int size)
        {
            int len = text.Length;
            
            string storingText = "";
            place.text = "";
            
            for (int i = 0; i < len; i++)
            {
                int x = 0;
                while (x <= size)
                {
                    place.text = storingText + "<size=" + x + ">" + text[i] + "</size>";
                    x++;
                    yield return new WaitForSeconds(0.00005f); 
                }
                storingText += text[i];
            }
        }
        
        public void TypeWriterText(string text, TMP_Text place, float velocity, bool withSound)
        {
            _typeWriterEnded = false;
            float time = 1f / velocity;
            StartCoroutine(TypeWriterCoroutine(text, place, time, withSound));
        }

        IEnumerator TypeWriterCoroutine(string text, TMP_Text place, float time, bool withSound)
        {
            int len = text.Length;
            place.text = "";
            for (int i = 0; i < len; i++)
            {
                place.text += text[i];
                if (withSound)
                {
                    AudioManager.instance.PlayClickKeyboard(); 
                }
                yield return new WaitForSeconds(time);
            }

            _typeWriterEnded = true;
        }
    }
}
