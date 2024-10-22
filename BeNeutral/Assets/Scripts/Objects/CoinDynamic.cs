using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Objects
{
    public class CoinDynamic : MonoBehaviour
    {

        public float destroyUpSpeed = 1;
        public bool animateUp = true;
        public Animator animator;

        private bool _isAnimatingDestroy;

        private void OnEnable()
        {
            _isAnimatingDestroy = false;
        }

        private void Update()
        {
            
            if (_isAnimatingDestroy)
            {
                transform.position += new Vector3(0, destroyUpSpeed * (animateUp ? 1 : -1) * Time.deltaTime, 0);
                float scaleDelta = destroyUpSpeed * 0.3f * Time.deltaTime;
                transform.localScale -= new Vector3(scaleDelta, scaleDelta, 1);
                animator.speed = 4;
                
                if (transform.localScale.x < 0.1)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            AudioManager.instance.PlayTakeCoinPlayer();
            _isAnimatingDestroy = true;
            gameObject.SetActive(false);
        }
    }
}