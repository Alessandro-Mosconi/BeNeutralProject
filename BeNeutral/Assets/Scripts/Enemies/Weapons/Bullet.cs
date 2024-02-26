using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Weapons
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 1;
        public float damage;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            /*float deltaX = gameObject.transform.parent.position.x - gameObject.transform.position.x;
            float deltaY = gameObject.transform.parent.position.y - gameObject.transform.position.y;
            if ( deltaX*deltaX + deltaY * deltaY> 20*20)
            {
                gameObject.SetActive(false);
            }*/
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) 
            {
                other.gameObject.GetComponent<PlayerManager>().DamagePlayer(damage);
                gameObject.SetActive(false);
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("MagneticField")) 
            {
                gameObject.SetActive(false);
            }
        }
    }
}
