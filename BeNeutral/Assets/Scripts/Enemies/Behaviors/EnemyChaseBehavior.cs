using UnityEngine;
using Attributes;

namespace Enemies.Behaviors
{
    public class EnemyChaseBehavior : EnemyBehavior
    {
        public float minTargetDistance = 0;
        public float maxTargetDistance = 1;
        public float movementSpeed = 1;
        public float cooldown = 2;
        public bool attacks = false;
        
        [ConditionalHide("attacks", true)]
        public float attackDecisionTimer = 1;
        [ConditionalHide("attacks", true)]
        [Range(0, 1)]
        public float attackProbability = 1;
        public float maxAttackDistance = 1;
        
        private Collider2D _targetCollider, _selfCollider;
        private float _halfSqrt2 = Mathf.Sqrt(2) * 0.5f;
        private float _cos30, _sin30, _cos60, _sin60;
        private int _playerLayerMask, _terrainRaycastLayermask;
        private float _cumulatedCooldownTime = 0;
        private float _cumulatedAttackTime = 0; 
        private bool _cooldownInProgress = false;
        private float _raycastVerticalSign = 1;
        
        public override EnemyBehaviorType Type()
        {
            return EnemyBehaviorType.Chase;
        }

        public override void ResetBehavior(Transform self)
        {
            _selfCollider = self.GetComponent<Collider2D>();
            _raycastVerticalSign = Mathf.Sign(transform.up.y);
            
            _cos30 = Mathf.Cos(30 * Mathf.Deg2Rad * _raycastVerticalSign);
            _sin30 = Mathf.Sin(30 * Mathf.Deg2Rad * _raycastVerticalSign);
            _cos60 = Mathf.Cos(60 * Mathf.Deg2Rad * _raycastVerticalSign);
            _sin60 = Mathf.Sin(60 * Mathf.Deg2Rad * _raycastVerticalSign);
            
            _playerLayerMask = LayerMask.GetMask("Player");
            _terrainRaycastLayermask = LayerMask.GetMask("Terrain");
            
            _cumulatedAttackTime = 0;
            if (!_cooldownInProgress)
            {
                _cumulatedCooldownTime = 0;
            }

            switchSignalCode = 0;
            
            base.ResetBehavior(self);
        }

        public override bool PerformStep(PlayerManager target, float deltaTime)
        {
            //1. Check if the player is visible to start any cooldown timers and then decide if we need to move back to Patrol mode
            CheckPlayerVisibility(target, deltaTime);
            //2. If a cooldown timer is in progress, update it
            bool chaseCooldownExpired = CheckCooldownExpired(deltaTime);
            //3. If the cooldown did not expire, keep chasing the player
            ChasePlayer(target, deltaTime);
            //4. Attack if needed
            bool shouldAttack = CheckAttack(target.transform, deltaTime);
            return chaseCooldownExpired || shouldAttack;
        }

        public override void DidAbandonState()
        {
            base.DidAbandonState();
        }

        private void CheckPlayerVisibility(PlayerManager target, float deltaTime)
        {
            TimeSinceLastUpdate += deltaTime;
            if (TimeSinceLastUpdate >= UpdateInterval && WeakSelf.TryGetTarget(out Transform self))
            {
                TimeSinceLastUpdate -= UpdateInterval;
                //Check if the enemy sees the player
                bool playerVisible = EnemyPatrolBehavior.IsPlayerVisible(self.position,
                    new Vector2(Mathf.Sign(self.localScale.x), 0), _cos30, _sin30, _cos60, _sin60, maxTargetDistance,
                    _playerLayerMask, UpdateInterval * 0.5f);
                if (playerVisible)
                {
                    //If the player is visible, reset any cooldown timers to keep moving
                    _cooldownInProgress = false;
                    _cumulatedCooldownTime = 0;
                }
                else if (!_cooldownInProgress)
                {
                    //If we don't see the player, start a cooldown timer
                    _cooldownInProgress = true;
                    _cumulatedCooldownTime = 0;
                }
            }
        }

        private bool CheckCooldownExpired(float deltaTime)
        {
            if (_cooldownInProgress)
            {
                _cumulatedCooldownTime += deltaTime;
                if (_cumulatedCooldownTime >= cooldown)
                {
                    _cooldownInProgress = false;
                    return true;
                }
            }

            return false;
        }

        private void ChasePlayer(PlayerManager target, float deltaTime)
        {
            //Chase the player until either we are too close to the player or to an obstacle
            if (WeakSelf.TryGetTarget(out Transform self))
            {
                Vector3 vecToTarget = target.transform.position - self.position;
                float distanceToTarget = Vector3.Magnitude(vecToTarget);
                Vector3 dirVector = vecToTarget / distanceToTarget;
                if (dirVector.x * self.localScale.x < 0)
                {
                    self.localScale = new Vector3(self.localScale.x * -1, self.localScale.y, self.localScale.z);
                }
                
                //Check for an obstacle in front
                bool obstacleAhead = HasObstacleInFront();

                if (distanceToTarget > minTargetDistance && !obstacleAhead)
                {
                    float scaledMovementSpeed = movementSpeed * deltaTime;
                    self.Translate(dirVector.x * scaledMovementSpeed, 0, 0);
                }
            }
        }

        private bool CheckAttack(Transform target, float deltaTime)
        {
            switchSignalCode = 0;
            if (attacks)
            {
                _cumulatedAttackTime += deltaTime;
                if (_cumulatedAttackTime >= attackDecisionTimer)
                {
                    _cumulatedAttackTime -= attackDecisionTimer;
                    if (WeakSelf.TryGetTarget(out Transform self))
                    {
                        Vector3 vecToTarget = target.position - self.position;
                        float distanceToTarget = Vector3.Magnitude(vecToTarget);
                        if (distanceToTarget <= maxAttackDistance && Random.Range(0, 1) <= attackProbability && IsGrounded())
                        {
                            switchSignalCode = 0xA77ACC0;
                            return true; //Switch to Attack mode! (this will stop the chase temporarily)
                        }
                    }
                }
            }

            return false;
        }
        
        private bool HasObstacleInFront()
        {
            //Check if we need to stop since we are about to hit an obstacle
            if (WeakSelf.TryGetTarget(out Transform self))
            {
                Vector2 position = self.position;
                Vector2 rightVector = new Vector2(Mathf.Sign(self.localScale.x), 0);
                Vector2 direction = rightVector;
            
                RaycastHit2D hit = Physics2D.Raycast(position, direction, minTargetDistance, _terrainRaycastLayermask | _playerLayerMask);
                Debug.DrawRay(position, direction * minTargetDistance, Color.blue);
                return hit.collider != null;
            }

            return false;
        }
        
        private bool IsGrounded() {
            if (WeakSelf.TryGetTarget(out Transform self))
            {
                Vector2 position = self.position;
                Vector2 direction = Vector2.down * _raycastVerticalSign;
            
                //Debug.DrawRay(position, direction * distance, Color.green);
                RaycastHit2D hit = Physics2D.Raycast(position, direction, 1.5f, _terrainRaycastLayermask);
                Debug.DrawRay(position, direction * 1.5f, Color.green, 0.5f);
                return hit.collider != null;
            }

            return false;
        }
    }
}