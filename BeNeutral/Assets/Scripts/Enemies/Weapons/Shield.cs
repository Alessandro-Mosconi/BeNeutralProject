using System;
using Player;
using UnityEngine;

namespace Enemies.Weapons
{
    public class Shield : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player Weapon"))
            {
                print("Collided with shield");
                //Bullet or weapon collided -> report collision without damaging the enemy
                BasePlayerWeapon otherObj = other.gameObject.GetComponent<BasePlayerWeapon>();
                otherObj.DidCollideWithEnemy();
            }
        }
    }
}