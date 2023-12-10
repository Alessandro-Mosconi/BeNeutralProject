using UnityEngine;

namespace Enemies.Behaviors
{
    public class EnemyShieldBehavior : EnemyBehavior
    {
        public GameObject shieldObject;

        public override void ResetBehavior(Transform self)
        {
            shieldObject.SetActive(true);
            base.ResetBehavior(self);
        }

        public override bool PerformStep(PlayerManager target, float deltaTime)
        {
            return false; //Transitions to other states must be controlled externally
        }

        public override void DidAbandonState()
        {
            shieldObject.SetActive(false);
        }
    }
}