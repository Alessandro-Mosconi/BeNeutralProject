using UnityEngine;

namespace UI
{
    public class TryAgain : MonoBehaviour
    {
        public void TryAgainFromThisLevel()
        {
            GameManager.instance.RestartThisLevel();
        }

        public void goBackToMainMenu()
        {
            GameManager.instance.ShowStartScreen();
        }
    }
}
