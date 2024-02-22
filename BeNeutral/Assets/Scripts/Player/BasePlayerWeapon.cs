using UI;
using UnityEngine;

namespace Player
{
    public abstract class BasePlayerWeapon : MonoBehaviour
    {
        public float damage = GameManager.instance.GetPlayerDamage();

        public abstract void DidCollideWithEnemy();
    }
}