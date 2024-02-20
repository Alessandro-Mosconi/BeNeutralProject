using UnityEngine;

namespace UI
{
    
    public class StartButton : MonoBehaviour
    {
            [Header("EASY")]
            [SerializeField] private int initialLivesE;
            [SerializeField] private int levelPointE;
            [SerializeField] private int damageLostPointE;
            [SerializeField] private int dieLostPointE;
            
            [Header("MEDIUM")]
            [SerializeField] private int initialLivesM;
            [SerializeField] private int levelPointM;
            [SerializeField] private int damageLostPointM;
            [SerializeField] private int dieLostPointM;
            
            [Header("HARD")]
            [SerializeField] private int initialLivesH;
            [SerializeField] private int levelPointH;
            [SerializeField] private int damageLostPointH;
            [SerializeField] private int dieLostPointH;
            
            
            public void Easy()
            {
                GameManager.instance.SetParameters(initialLivesE, levelPointE, damageLostPointE, dieLostPointE);
                GameManager.instance.StartGame();
            }
            
            public void Medium()
            {
                GameManager.instance.SetParameters(initialLivesM, levelPointM, damageLostPointM, dieLostPointM);
                GameManager.instance.StartGame();
            }
            
            public void Hard()
            {
                GameManager.instance.SetParameters(initialLivesH, levelPointH, damageLostPointH, dieLostPointH);
                GameManager.instance.StartGame();
            }
    
    }
}
