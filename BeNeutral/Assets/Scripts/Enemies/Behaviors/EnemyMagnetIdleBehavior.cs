using Enemies.Weapons;
using UnityEngine;

namespace Enemies.Behaviors
{
    public class EnemyMagnetIdleBehavior : EnemyBehavior
    {
        public float idleDuration = 1; //Seconds

        private float _idleTime = 0;
        
        public override EnemyBehaviorType Type()
        {
            return EnemyBehaviorType.MagnetIdle;
        }
        
        public override void ResetBehavior(Transform self)
        {
            _idleTime = 0;
            
            base.ResetBehavior(self);
        }

        public override bool PerformStep(PlayerManager target, float deltaTime)
        {
            _idleTime += deltaTime;
            if (_idleTime >= idleDuration)
            {
                return true; //Switch to MagnetActive
            }

            return false;
        }

        public override void DidAbandonState()
        {
            base.DidAbandonState();
        }
    }
}