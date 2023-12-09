using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;


public class ScoreManager : Singleton<ScoreManager>
    {
        [SerializeField] private TextMeshProUGUI pointsDisplay;
        [SerializeField] private TextMeshProUGUI lifesDisplay;
        private int points = 0;
        private int lifes;
        private int checkpointScore = 0;
            
       
        

        public void UpdateScore(int n)
        {
            points = n;
            pointsDisplay.text = string.Format("{0:D6}", n);
        }

        public void SetLifes(int n)
        {
            lifes = n;
            lifesDisplay.text = string.Format("{0:D1}", lifes);
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
            return lifes != 0;
        }

        public void AddToScore(int n)
        {
            points += n;
            pointsDisplay.text = string.Format("{0:D6}", points);
        }
        
        public void SubToScore(int n)
        {
            points -= n;
            pointsDisplay.text = string.Format("{0:D6}", points);
        }

        public void SaveLastScore()
        {
            checkpointScore = points;
        }

        public void RestoreScore()
        {
            points = checkpointScore;
            pointsDisplay.text = string.Format("{0:D6}", points);
        }

        public void ResetScore()
        {
            points = 0;
            checkpointScore = 0;
            pointsDisplay.text = string.Format("{0:D6}", points);
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
