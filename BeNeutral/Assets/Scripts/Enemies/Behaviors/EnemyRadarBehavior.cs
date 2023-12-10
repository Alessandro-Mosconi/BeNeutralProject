using UnityEngine;

namespace Enemies.Behaviors
{
    public class EnemyRadarBehavior : EnemyBehavior
    {
        public float radarSpeed = 1;
        public float visibilityRange = 10;

        private int _playerLayerMask;
        private float _currentAlpha;
        private Vector2 _currentRadarDir;

        public override void ResetBehavior(Transform self)
        {
            _playerLayerMask = LayerMask.GetMask("Player");
            _currentRadarDir = new Vector2(1, 0);
            _currentAlpha = 0;
            
            base.ResetBehavior(self);
        }

        public override bool PerformStep(PlayerManager target, float deltaTime)
        {
            //1. Decide whether we need to stay in this state or switch to action. Decision is taken based on the policy update interval
            bool shouldSwitchToAction = ShouldSwitchToAction(deltaTime);
            //2. If we stay in this state, keep patrolling
            if (!shouldSwitchToAction)
            {
                RunRadarActions(deltaTime);
            }
            return shouldSwitchToAction;
        }

        public override void DidAbandonState()
        {
            base.DidAbandonState();
        }

        private bool ShouldSwitchToAction(float deltaTime)
        {
            //Decides whether to stay in Radar mode or switch to action
            //NB: If the player escapes, then the policy will move the enemy back to Radar mode!
            TimeSinceLastUpdate += deltaTime;
            if (TimeSinceLastUpdate >= UpdateInterval && WeakSelf.TryGetTarget(out Transform self))
            {
                TimeSinceLastUpdate -= UpdateInterval;
                _currentRadarDir.x = Mathf.Cos(_currentAlpha);
                _currentRadarDir.y = Mathf.Sin(_currentAlpha);
                return IsPlayerVisible(self.position, _currentRadarDir, visibilityRange, _playerLayerMask, UpdateInterval * 0.5f);
            }

            return false;
        }

        public static bool IsPlayerVisible(Vector2 position, Vector2 direction, float visibilityRange, int playerLayerMask, float rayVisibility)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, direction, visibilityRange, playerLayerMask);
            Debug.DrawRay(position, direction * visibilityRange, Color.red, rayVisibility);

            return hit.collider != null;
        }

        private void RunRadarActions(float deltaTime)
        {
            _currentAlpha += (deltaTime * radarSpeed * Mathf.Deg2Rad);
            if (_currentAlpha > 2 * Mathf.PI)
            {
                _currentAlpha -= 2 * Mathf.PI;
            }
        }
    }
}