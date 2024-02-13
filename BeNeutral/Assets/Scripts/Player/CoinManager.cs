using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "CoinManager", menuName = "Coin Manager", order = 0)]
    public class CoinManager : ScriptableObject
    {
        [SerializeField] private int collectedCoins;
        [SerializeField] private int coinsForLife = 50;
        private int creditedLives = 0;

        public int CollectedCoins
        {
            get => collectedCoins;
            set
            {
                int numCoins = value - collectedCoins;
                collectedCoins = value;
                ScoreManager.instance.AddCoins(numCoins);
            }
        }

        public int CreditLife()
        {
            int lives = (collectedCoins / coinsForLife) - creditedLives;
            creditedLives += lives;
            return lives;
        }
    }
}