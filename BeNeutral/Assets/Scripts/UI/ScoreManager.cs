using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        [SerializeField] private TextMeshProUGUI lifeDisplay;


        public void UpdateLifes(int n)
        {
            lifeDisplay.text = string.Format("{0:D6}", n);
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
}