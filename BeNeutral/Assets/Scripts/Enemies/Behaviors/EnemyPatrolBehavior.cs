using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.Behaviors
{
    public class EnemyPatrolBehavior : EnemyBehavior
    {
        public float maxCoveredDistance = 1;
        public float patrolSpeed = 1;
        public float visibilityRange = 10;

        private float _directionSign = 1;
        private float _cumulatedCoveredDirection = 0; //When it reaches +- maxCoveredDistance * 0.5f we switch direction!
        private float _terrainCheckRaycastLength = 0;
        private float _obstacleCheckRaycastLength = 0;
        private int _terrainRaycastLayermask, _obstacleRaycastLayermask, _playerLayerMask;
        
        private float _cos30, _sin30, _cos60, _sin60;

        public override void ResetBehavior(Transform self)
        {
            _terrainCheckRaycastLength = self.GetComponent<Collider2D>().bounds.extents.y + 0.2f;
            _terrainRaycastLayermask = LayerMask.GetMask("Terrain");
            _obstacleCheckRaycastLength = self.GetComponent<Collider2D>().bounds.extents.x + 0.6f;
            _obstacleRaycastLayermask = LayerMask.GetMask("Damage-dealing", "Terrain", "Default");
            _playerLayerMask = LayerMask.GetMask("Player");

            _cos30 = Mathf.Cos(30 * Mathf.Deg2Rad);
            _sin30 = Mathf.Sin(30 * Mathf.Deg2Rad);
            _cos60 = Mathf.Cos(60 * Mathf.Deg2Rad);
            _sin60 = Mathf.Sin(60 * Mathf.Deg2Rad);
            
            _directionSign = Mathf.Sign(self.localScale.x);
            _cumulatedCoveredDirection = 0;
            
            base.ResetBehavior(self);
        }

        public override bool PerformStep(PlayerManager target, float deltaTime)
        {
            //1. Decide whether we need to stay in this state or switch to active Follow. Decision is taken based on the policy update interval
            bool shouldSwitchToFollow = ShouldSwitchToFollow(deltaTime);
            //2. If we stay in this state, keep patrolling
            if (!shouldSwitchToFollow)
            {
                RunPatrollingActions(deltaTime);
            }
            return shouldSwitchToFollow;
        }

        public override void DidAbandonState()
        {
            base.DidAbandonState();
        }

        private bool ShouldSwitchToFollow(float deltaTime)
        {
            //Decides whether to stay in Patrol mode or switch to active follow
            //NB: If the player escapes, then the policy will move the enemy back to patrol mode!
            TimeSinceLastUpdate += deltaTime;
            if (TimeSinceLastUpdate >= UpdateInterval && WeakSelf.TryGetTarget(out Transform self))
            {
                TimeSinceLastUpdate -= UpdateInterval;
                return IsPlayerVisible(self.position, new Vector2(_directionSign, 0), _cos30, _sin30, _cos60, _sin60, visibilityRange, _playerLayerMask, UpdateInterval * 0.5f);
            }

            return false;
        }

        public static bool IsPlayerVisible(Vector2 position, Vector2 direction, float cosAlpha1, float sinAlpha1, float cosAlpha2, float sinAlpha2, float visibilityRange, int playerLayerMask, float rayVisibility)
        {
            Vector2 direction30 = (new Vector2(direction.x * cosAlpha1 - direction.y * sinAlpha1, (direction.x * sinAlpha1 + direction.y * cosAlpha1) * direction.x));
            Vector2 direction60 = (new Vector2(direction.x * cosAlpha2 - direction.y * sinAlpha2, (direction.x * sinAlpha2 + direction.y * cosAlpha2) * direction.x));
            
            //Debug.DrawRay(position, direction * distance, Color.green);
            RaycastHit2D hitHoriz = Physics2D.Raycast(position, direction, visibilityRange, playerLayerMask);
            RaycastHit2D hit30 = Physics2D.Raycast(position, direction30, visibilityRange, playerLayerMask);
            RaycastHit2D hit60 = Physics2D.Raycast(position, direction60, visibilityRange, playerLayerMask);
            Debug.DrawRay(position, direction * visibilityRange, Color.red, rayVisibility);
            Debug.DrawRay(position, direction30 * visibilityRange, Color.red, rayVisibility);
            Debug.DrawRay(position, direction60 * visibilityRange, Color.red, rayVisibility);
            bool playerVisible = hitHoriz.collider != null || hit30.collider != null || hit60.collider != null;

            return playerVisible;
        }

        private void RunPatrollingActions(float deltaTime)
        {
            if (WeakSelf.TryGetTarget(out Transform self))
            {
                if (_cumulatedCoveredDirection >= maxCoveredDistance * 0.5f || ShouldChangeDirection())
                {
                    _cumulatedCoveredDirection = 0;
                    self.localScale = new Vector3(self.localScale.x * -1, self.localScale.y, self.localScale.z);
                    _directionSign *= -1;
                }

                _cumulatedCoveredDirection += patrolSpeed * deltaTime;
                Vector2 forwardDir = new Vector2(_directionSign, 0);
                self.Translate(patrolSpeed * deltaTime * forwardDir);
            }
        }
        
        private bool ShouldChangeDirection()
        {
            //Check if we need to change direction since we are about to fall or hit an obstacle
            //Obstacle in front
            if (WeakSelf.TryGetTarget(out var self))
            {
                Vector2 position = self.position;
                Vector2 rightVector = new Vector2(_directionSign, 0);
                Vector2 direction = rightVector;
            
                RaycastHit2D[] hits = Physics2D.RaycastAll(position, direction, _obstacleCheckRaycastLength, _obstacleRaycastLayermask);
                Debug.DrawRay(position, direction * _obstacleCheckRaycastLength, Color.green);
                bool obstacleFound = false;
                for (int i = 0; i < hits.Length && !obstacleFound; i++)
                {
                    if (!hits[i].transform.IsChildOf(self))
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

            return false;
        }
    }
}