using System;
using UnityEngine;
using UnityEngine.Serialization;
using Enemies.Weapons;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class HammeringEnemy : EnemyPolicy
    {

        public float maxHammeringDistance = 10;
        public float hammerActionProbability = 0.6f;
        public HammerComponent hammer;
        public GameObject shieldObject;
        
        private EnemyFollowPlayerPolicy _followPlayerPolicy;
        private bool _canMove;
        private int _groundLayer;
        private float _groundRaycastDistance;
        private bool _shields;
        private GameObject _hammerObject;
        
        protected override void OnPolicyStart()
        {
            _followPlayerPolicy = GetComponent<EnemyFollowPlayerPolicy>();
            _canMove = _followPlayerPolicy != null;
            _groundLayer = LayerMask.GetMask("Terrain");
            _groundRaycastDistance = GetComponent<Collider2D>().bounds.extents.y + 0.2f;
            _shields = shieldObject != null;
            _hammerObject = hammer.gameObject;
            hammer.enabled = false;
            SwitchHammerShield(false);
            hammer.OnHammeringStart = () =>
            {
                if (_canMove)
                {
                    _followPlayerPolicy.enabled = false;
                }
                SwitchHammerShield(true);
            };
            hammer.OnHammeringEnd = () =>
            {
                if (_canMove)
                {
                    _followPlayerPolicy.enabled = true;
                    //Reset hammer timer
                    ResetPolicyExecution();
                }
                SwitchHammerShield(false);
            };
        }

        protected override bool PolicyShouldDecide()
        {
            float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
            bool hammeringAllowed =
                distanceToPlayer <= maxHammeringDistance && Random.Range(0, 1) <= hammerActionProbability;
            //Avoid hammering when we are about to fall
            if (hammeringAllowed)
            {
                hammeringAllowed = IsGrounded();
            }
            return hammeringAllowed;
        }

        protected override void ExecutePolicy()
        {
            if (!_hammerObject.activeSelf)
            {
                SwitchHammerShield(true);
            }
            hammer.enabled = true;
        }
        
        private bool IsGrounded() {
            Vector2 position = transform.position;
            Vector2 direction = Vector2.down;
            
            //Debug.DrawRay(position, direction * distance, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(position, direction, _groundRaycastDistance, _groundLayer);
            Debug.DrawRay(position, direction * _groundRaycastDistance, Color.green, 0.5f);
            return hit.collider != null;
        }

        private void SwitchHammerShield(bool hammerActive)
        {
            if (_shields)
            {
                _hammerObject.SetActive(hammerActive);
                shieldObject.SetActive(!hammerActive);
            }
        }
    }
}