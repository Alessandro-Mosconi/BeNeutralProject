using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.Behaviors
{
    public class EnemyAbsorbsChargeBehavior : EnemyBehavior
    {

        public float maxDistance = 2;
        public float maxDamagePerSecond = 1;
        public AnimationCurve damageCurve = new AnimationCurve(new []{new Keyframe(0, 1), new Keyframe(1, 0)});

        private ParticleSystemForceField _particleSystemForceField;
        private ParticleSystem _playerParticleSystem;
        private bool _particleSystemActive = false;

        public override EnemyBehaviorType Type()
        {
            return EnemyBehaviorType.AbsorbsCharge;
        }

        public override void ResetBehavior(Transform self)
        {
            if (_particleSystemForceField == null)
            {
                _particleSystemForceField = gameObject.AddComponent<ParticleSystemForceField>();
                _particleSystemForceField.shape = ParticleSystemForceFieldShape.Sphere;
                _particleSystemForceField.endRange = maxDistance;
                _particleSystemForceField.gravity = new ParticleSystem.MinMaxCurve(0.1f);
            }
        }

        public override bool PerformStep(PlayerManager target, float deltaTime)
        {
            if (WeakSelf.TryGetTarget(out Transform self))
            {
                Vector3 vecToTarget = target.transform.position - self.position;
                float distanceToTarget = Vector3.Magnitude(vecToTarget);

                if (distanceToTarget <= maxDistance)
                {
                    float damageCurveValue = damageCurve.Evaluate(distanceToTarget / maxDistance);
                    float damage = damageCurveValue * maxDamagePerSecond * deltaTime;
                    target.DamagePlayer(damage);
                    if (!_particleSystemActive)
                    {
                        FindPlayerParticleSystem(target);
                        SetParticleSystemActive(true);
                    }
                    //Direct the particle system towards the enemy
                    if (_playerParticleSystem != null)
                    {
                        var particleEmission = _playerParticleSystem.emission;
                        particleEmission.rateOverTime = new ParticleSystem.MinMaxCurve(30 *
                            (1 - (distanceToTarget / maxDistance)));
                        _playerParticleSystem.transform.rotation = 
                            Quaternion.AngleAxis(Mathf.Atan2(vecToTarget.y, vecToTarget.x) * Mathf.Rad2Deg, new Vector3(1, 0, 0));
                    }
                }
                else
                {
                    if (_particleSystemActive)
                    {
                        FindPlayerParticleSystem(target);
                        SetParticleSystemActive(false);
                    }
                }
            }
            return false;
        }

        public override void DidAbandonState()
        {
            base.DidAbandonState();
        }

        private void FindPlayerParticleSystem(PlayerManager target)
        {
            if (_playerParticleSystem == null)
            {
                foreach(Transform child in target.transform)
                {
                    if (child.gameObject.CompareTag("Particle"))
                    {
                        _playerParticleSystem = child.GetComponent<ParticleSystem>();
                        break;
                    }
                }
            }
        }

        private void SetParticleSystemActive(bool active)
        {
            if (_playerParticleSystem != null)
            {
                if (active)
                {
                    _playerParticleSystem.Play();
                    _particleSystemActive = true;
                }
                else
                {
                    _playerParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    _particleSystemActive = false;
                }
            }
        }
        
    }
}