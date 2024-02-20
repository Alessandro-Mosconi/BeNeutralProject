using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;


public class ScoreManager : Singleton<ScoreManager>
    {
        [SerializeField] private TextMeshProUGUI pointsDisplay;
        [SerializeField] private TextMeshProUGUI coinsDisplay;
        [SerializeField] private TextMeshProUGUI lifesDisplay;
        [SerializeField] private TextMeshProUGUI timeDisplay;
        private int points = 0;
        private int coins = 0;
        private int lifes;
        private int checkpointScore = 0;
        [SerializeField] private Image lifeImage;



        public int GetPoints()
        {
            return points;
        }
        public int GetCoins()
        {
            return coins;
        }
        public void UpdateScore(int n)
        {
            points = n;
            pointsDisplay.text = string.Format("{0:D5}", n);
        }

        public void UpdateTime()
        {
           float t = GameManager.instance.ActualTimeLevel();
           float min = MathF.Floor(t / 60) ;
           float sec = MathF.Round(t-(min*60), 0);
           
           if (sec < 10)
           {
               if (min < 10)
               {
                   timeDisplay.text = "0" + min + ".0" + sec; 
               }
               else
               {
                   timeDisplay.text = min + ".0" + sec;
               }
               
           }
           else
           {
               if (min < 10)
               {
                   timeDisplay.text = "0" + min + "." + sec; 
               }
               else
               {
                   timeDisplay.text = min + "." + sec;
               }
           }
        }

        public void SetLifes(int n)
        {
            lifes = n;
            lifesDisplay.text = string.Format("{0:D1}", lifes);
            lifeImage.fillAmount = Mathf.Clamp((float) lifes / GameManager.instance.GetStartingLifes(), 0, GameManager.instance.GetStartingLifes());
        }

        public int GetLifes()
        {
            return lifes;
        }

        //return true if has more than 0 lifes
        public bool LoseOneLife()
        {
            lifes--;
            lifesDisplay.text = string.Format("{0:D1}", lifes);
            lifeImage.fillAmount = Mathf.Clamp((float) lifes / GameManager.instance.GetStartingLifes(), 0, GameManager.instance.GetStartingLifes());
            return lifes != 0;
        }

        public void AddToScore(int n)
        {
            points += n;
            pointsDisplay.text = string.Format("{0:D5}", points);
        }
        
        public void AddCoins(int n)
        {
            coins += n;
            coinsDisplay.text = $"{coins:D5}";
        }
        
        public void SubToScore(int n)
        {   
            points -= n;
            if (points < 0)
            {
                points = 0;
            }
            pointsDisplay.text = string.Format("{0:D5}", points);
        }

        public void SaveLastScore()
        {
            checkpointScore = points;
        }

        public void RestoreScore()
        {
            points = checkpointScore;
            pointsDisplay.text = string.Format("{0:D5}", points);
        }

        public void ResetScore()
        {
            points = 0;
            checkpointScore = 0;
            coins = 0;
            pointsDisplay.text = string.Format("{0:D5}", points);
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
