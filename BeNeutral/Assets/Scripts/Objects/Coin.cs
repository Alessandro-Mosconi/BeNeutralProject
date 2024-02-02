using System;
using UnityEngine;

namespace Objects
{
    public class Coin : MonoBehaviour
    {

        public float destroyUpSpeed = 1;
        public Animator animator;

        private bool _isAnimatingDestroy = false;
        
        private void Update()
        {
            if (_isAnimatingDestroy)
            {
                transform.position += new Vector3(0, destroyUpSpeed * Time.deltaTime, 0);
                float scaleDelta = destroyUpSpeed * 0.3f * Time.deltaTime;
                transform.localScale -= new Vector3(scaleDelta, scaleDelta, 1);
                animator.speed = 3;

                if (transform.localScale.x < 0.1)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _isAnimatingDestroy = true;
        }
    }
}