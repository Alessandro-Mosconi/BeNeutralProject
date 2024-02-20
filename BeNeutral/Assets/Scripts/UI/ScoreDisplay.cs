using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        
        public void Start()
        {
            int x = 0;
            x = ScoreManager.Instance.GetPoints();
            text.text = string.Format("{0:D6}", x);
        }
    }
}
