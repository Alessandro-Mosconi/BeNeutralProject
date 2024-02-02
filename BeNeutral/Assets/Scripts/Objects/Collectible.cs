using UnityEngine;

namespace Objects
{
    public class Collectible : MonoBehaviour
    {
        private static Collectible instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}