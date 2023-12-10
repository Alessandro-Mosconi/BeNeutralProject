using System;
using Player;
using UnityEngine;

namespace Enemies.Weapons
{
    public class Shield : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.GetMask("Player Weapon"))
            {
                //Bullet or weapon collided -> report collision without damaging the enemy
                BasePlayerWeapon otherObj = other.gameObject.GetComponent<BasePlayerWeapon>();
                otherObj.DidCollideWithEnemy();
            }
        }
    }
}