using UnityEngine;
using Enemies.Weapons;

namespace Enemies.Behaviors
{
    public class EnemyHammerBehavior : EnemyBehavior
    {
        public HammerComponent hammer;
        public float damage = 2;
        
        private GameObject _hammerObject;
        private bool _hammeringActionCompleted;

        public override void ResetBehavior(Transform self)
        {
            _hammerObject = hammer.gameObject;
            hammer.enabled = true;
            _hammerObject.SetActive(true);
            _hammeringActionCompleted = false;
            hammer.damage = damage;
            
            hammer.OnHammeringStart = () =>
            {
                //Do nothing, we don't need it!
                _hammeringActionCompleted = false;
            };
            hammer.OnHammeringEnd = () =>
            {
                //Encode the state switch
                _hammeringActionCompleted = true;
            };
            
            base.ResetBehavior(self);
        }

        public override bool PerformStep(GameObject target, float deltaTime)
        {
            return _hammeringActionCompleted;
        }

        public override void DidAbandonState()
        {
            hammer.ResetHammer();
            _hammerObject.SetActive(false);
        }
    }
}