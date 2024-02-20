using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text points;
        [SerializeField] private TMP_Text coins;
        [SerializeField] private TMP_Text time;
        
        public void Start()
        {
            int p = 0;
            int c = 0;
            p = ScoreManager.Instance.GetPoints();
            c = ScoreManager.Instance.GetCoins();
            points.text = string.Format("{0:D5}", p);
            coins.text = string.Format("{0:D5}", c);

            FinalTimeCalculation();
        }

        public void FinalTimeCalculation()
        {
            float t = GameManager.instance.FinalTimeLevel();
            float min = MathF.Floor(t / 60) ;
            float sec = MathF.Round(t-(min*60), 0);
           
            if (sec < 10)
            {
                if (min < 10)
                {
                    time.text = "0" + min.ToString() + ".0" + sec.ToString(); 
                }
                else
                {
                    time.text = min.ToString() + ".0" + sec.ToString();
                }
               
            }
            else
            {
                if (min < 10)
                {
                    time.text = "0" + min.ToString() + "." + sec.ToString(); 
                }
                else
                {
                    time.text = min.ToString() + "." + sec.ToString();
                }
            }
        }
    }
}
