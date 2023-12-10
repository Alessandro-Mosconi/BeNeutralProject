using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.Behaviors
{
    public class EnemyAbsorbsChargeBehavior : EnemyBehavior
    {

        public float maxDistance = 5;
        public float maxDamagePerSecond = 2;
        public AnimationCurve damageCurve = new AnimationCurve(new []{new Keyframe(0, 1), new Keyframe(1, 0)});

        public override void ResetBehavior(Transform self)
        {
            base.ResetBehavior(self);
        }

        public override bool PerformStep(PlayerManager target, float deltaTime)
        {
            if (WeakSelf.TryGetTarget(out Transform self))
            {
                Vector3 vecToTarget = target.transform.position - self.position;
                float distanceToTarget = Vector3.Magnitude(vecToTarget);

                if (distanceToTarget <= maxDistance)
                {
                    float damage = damageCurve.Evaluate(1 - (distanceToTarget / maxDistance)) * maxDamagePerSecond * deltaTime;
                    target.DamagePlayer(damage);
                }
            }
            return false;
        }

        public override void DidAbandonState()
        {
            base.DidAbandonState();
        }
        
        
    }
}