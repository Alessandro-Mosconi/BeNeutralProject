using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyPatrolPolicy : EnemyPolicy
    {

        public float maxCoveredDistance = 1;
        public float patrolSpeed = 1;
        public float playerVisibleDistance = 10;
        public float cooldownAfterPlayerDisappears = 1;

        private List<EnemyPolicy> _activePoliciesAfterPatrol;
        private float _directionSign = 1;
        private float _cumulatedCoveredDirection = 0; //When it reaches +- maxCoveredDistance * 0.5f we switch direction!
        private float _cumulatedCooldownTime = 0;
        private bool _isPatrolling = true;
        private bool _cooldownStarted = false;
        private float _terrainCheckRaycastLength = 0;
        private float _obstacleCheckRaycastLength = 0;
        private int _terrainRaycastLayermask, _obstacleRaycastLayermask, _playerLayerMask;
        
        private float _cos30, _sin30, _cos60, _sin60;
        
        protected override void OnPolicyStart()
        {
            _activePoliciesAfterPatrol = new List<EnemyPolicy>(GetComponents<EnemyPolicy>());
            //Remove self from list
            _activePoliciesAfterPatrol.Remove(this);
            foreach (EnemyPolicy enemyPolicy in _activePoliciesAfterPatrol)
            {
                enemyPolicy.enabled = false;
            }
            
            _terrainCheckRaycastLength = GetComponent<Collider2D>().bounds.extents.y + 0.2f;
            _terrainRaycastLayermask = LayerMask.GetMask("Terrain");
            _obstacleCheckRaycastLength = GetComponent<Collider2D>().bounds.extents.x + 0.6f;
            _obstacleRaycastLayermask = LayerMask.GetMask("Damage-dealing", "Terrain", "Default");
            _playerLayerMask = LayerMask.GetMask("Player");

            _cos30 = Mathf.Cos(30 * Mathf.Deg2Rad);
            _sin30 = Mathf.Sin(30 * Mathf.Deg2Rad);
            _cos60 = Mathf.Cos(60 * Mathf.Deg2Rad);
            _sin60 = Mathf.Sin(60 * Mathf.Deg2Rad);
        }

        protected override bool PolicyShouldDecide()
        {
            //Decides whether to stay in Patrol mode or switch to active follow
            //NB: If the player escapes, then the policy will move the enemy back to patrol mode!
            Vector2 position = transform.position;
            Vector2 direction = new Vector2(_directionSign, 0);
            Vector2 direction30 = (new Vector2(direction.x * _cos30 - direction.y * _sin30, (direction.x * _sin30 + direction.y * _cos30) * _directionSign));
            Vector2 direction60 = (new Vector2(direction.x * _cos60 - direction.y * _sin60, (direction.x * _sin60 + direction.y * _cos60) * _directionSign));
            
            //Debug.DrawRay(position, direction * distance, Color.green);
            RaycastHit2D hitHoriz = Physics2D.Raycast(position, direction, playerVisibleDistance, _playerLayerMask);
            RaycastHit2D hit30 = Physics2D.Raycast(position, direction30, playerVisibleDistance, _playerLayerMask);
            RaycastHit2D hit60 = Physics2D.Raycast(position, direction60, playerVisibleDistance, _playerLayerMask);
            Debug.DrawRay(position, direction * playerVisibleDistance, Color.red, updateInterval);
            Debug.DrawRay(position, direction30 * playerVisibleDistance, Color.red, updateInterval);
            Debug.DrawRay(position, direction60 * playerVisibleDistance, Color.red, updateInterval);
            bool playerVisible = hitHoriz.collider != null || hit30.collider != null || hit60.collider != null;

            if (_isPatrolling && playerVisible)
            {
                //Start acting!
                foreach (EnemyPolicy enemyPolicy in _activePoliciesAfterPatrol)
                {
                    enemyPolicy.enabled = true;
                }

                _isPatrolling = false;
                _cooldownStarted = false;
                _cumulatedCooldownTime = 0;
            } else if (!_isPatrolling && !playerVisible && !_cooldownStarted)
            {
                //Start cooldown timer, then move back to patrolling once timer expires
                _cooldownStarted = true;
                _cumulatedCooldownTime = 0;
            }

            return true; //Always active policy
        }

        protected override void ExecutePolicy()
        {
            //A patrolling policy moves back and forth at fixed speed without falling from the platform the enemy is placed on
            if (_isPatrolling)
            {
                if (Mathf.Abs(_cumulatedCoveredDirection) >= maxCoveredDistance * 0.5f || ShouldChangeDirection())
                {
                    _cumulatedCoveredDirection = 0;
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    _directionSign *= -1;
                }

                _cumulatedCoveredDirection += patrolSpeed * Time.deltaTime;
                Vector2 forwardDir = new Vector2(_directionSign, 0);
                transform.Translate(patrolSpeed * Time.deltaTime * forwardDir);
            }
            else
            {
                if (_cooldownStarted)
                {
                    _cumulatedCooldownTime += Time.deltaTime;
                    if (_cumulatedCooldownTime >= cooldownAfterPlayerDisappears)
                    {
                        _isPatrolling = true;
                        _cooldownStarted = false;
                        foreach (EnemyPolicy enemyPolicy in _activePoliciesAfterPatrol)
                        {
                            enemyPolicy.enabled = false;
                        }
                    }
                }
            }
        }
        
        private bool ShouldChangeDirection()
        {
            //Check if we need to change direction since we are about to fall or hit an obstacle
            //Obstacle in front
            Vector2 position = transform.position;
            Vector2 rightVector = new Vector2(_directionSign, 0);
            Vector2 direction = rightVector;
            
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, direction, _obstacleCheckRaycastLength, _obstacleRaycastLayermask);
            Debug.DrawRay(position, direction * _obstacleCheckRaycastLength, Color.green);
            bool obstacleFound = false;
            for (int i = 0; i < hits.Length && !obstacleFound; i++)
            {
                if (!hits[i].transform.IsChildOf(transform))
                {
                    obstacleFound = true;
                }
            }

            direction = Vector2.down;
            position += (rightVector * 0.3f);
            RaycastHit2D hitTerrain = Physics2D.Raycast(position, direction, _terrainCheckRaycastLength, _terrainRaycastLayermask);
            Debug.DrawRay(position, direction * _terrainCheckRaycastLength, Color.green);
            return obstacleFound || hitTerrain.collider == null;
        }
    }
}