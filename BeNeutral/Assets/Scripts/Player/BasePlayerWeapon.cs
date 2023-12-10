using UnityEngine;

namespace Player
{
    public abstract class BasePlayerWeapon : MonoBehaviour
    {
        public float damage = 2;

        public abstract void DidCollideWithEnemy();
    }
}