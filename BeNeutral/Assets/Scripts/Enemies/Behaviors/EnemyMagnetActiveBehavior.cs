using Enemies.Weapons;
using UnityEngine;

namespace Enemies.Behaviors
{
    public class EnemyMagnetActiveBehavior : EnemyBehavior
    {
        public float activeDuration = 1; //Seconds
        public float fieldIntensity = 1;
        public EnemyMagneticField.Polarity polarity;
        [ColorUsage(showAlpha: true, hdr: true)] public Color blueColor, redColor;
        public EnemyMagneticField magneticField;
        
        private float _activeTime = 0;
        private SpriteRenderer _renderer;
        private EnemyAbsorbsChargeBehavior _absorbsChargeBehavior;
        
        public override EnemyBehaviorType Type()
        {
            return EnemyBehaviorType.MagnetActive;
        }
        
        public override void ResetBehavior(Transform self)
        {
            _activeTime = 0;

            magneticField.polarity = polarity;
            magneticField.blueColor = blueColor;
            magneticField.redColor = redColor;
            magneticField.baseFieldIntensity = fieldIntensity;
            magneticField.OnEffectApplicationChange = OnMagneticFieldApplicationChange;
            magneticField.enabled = true;
            magneticField.gameObject.SetActive(true);
            magneticField.UpdateShaderProperties();

            if (!_renderer)
            {
                _renderer = GetComponent<SpriteRenderer>();
            }
            _renderer.color = polarity == EnemyMagneticField.Polarity.Red ? redColor : blueColor;

            if (!_absorbsChargeBehavior)
            {
                _absorbsChargeBehavior = GetComponent<EnemyAbsorbsChargeBehavior>();
            }
            
            base.ResetBehavior(self);
        }

        public override bool PerformStep(PlayerManager target, float deltaTime)
        {
            _activeTime += deltaTime;
            
            if (_activeTime >= activeDuration)
            {
                return true; //Switch to MagnetIdle
            }

            return false;
        }

        public override void DidAbandonState()
        {
            magneticField.enabled = false;
            magneticField.gameObject.SetActive(false);
            _renderer.color = Color.white;
            _absorbsChargeBehavior.SetParticleSystemActive(false);
            
            base.DidAbandonState();
        }

        private void OnMagneticFieldApplicationChange(bool appliesToPlayer)
        {
            _absorbsChargeBehavior.absorptionEnabled = appliesToPlayer;
        }
    }
}