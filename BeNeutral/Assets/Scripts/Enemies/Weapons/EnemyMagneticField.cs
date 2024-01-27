using System;
using UnityEngine;

namespace Enemies.Weapons
{
    public class EnemyMagneticField : MonoBehaviour
    {
        public enum Polarity
        {
            Blue,
            Red
        };
        
        [HideInInspector] public Polarity polarity;
        [HideInInspector] [ColorUsage(showAlpha: true, hdr: true)] public Color blueColor, redColor;
        [HideInInspector] public EnemyMagneticField magneticField;
        public AnimationCurve fieldStrengthCurve;
        [HideInInspector] public float baseFieldIntensity;
        [HideInInspector] public Action<bool> OnEffectApplicationChange;

        private bool _applyForceField = false;
        private Rigidbody2D _targetRb;
        private PlayerManager _targetPm;
        private PlayerMovement _targetPlayerMovement;
        private float _fieldExtent;
        private bool _prevEffectAppliedToPlayer = false;

        private Material _material;

        private void Start()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _fieldExtent = transform.localScale.x * 0.5f;
                //Apply field force
                _applyForceField = true;
                if (_targetRb != other.attachedRigidbody)
                {
                    _targetRb = other.attachedRigidbody;
                    _targetPm = other.GetComponent<PlayerManager>();
                    _targetPlayerMovement = other.GetComponent<PlayerMovement>();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.attachedRigidbody == _targetRb)
            {
                _applyForceField = false;
                _material.SetFloat("_Mask_Enable", 0);
            }
        }

        private void FixedUpdate()
        {
            if (_applyForceField)
            {
                //Apply repulsive/attractive field depending on player polarity and whether their magnetic field is active or not
                bool playerFieldActive = _targetPm.Field.GetCurrentIntensity() > 0.1;
                Polarity playerPolarity = _targetPm.Field.playerPolarity > 0 ? Polarity.Red : Polarity.Blue;
                bool fieldAttractive = playerPolarity != polarity;
                bool effectAppliedToPlayer = !playerFieldActive || !fieldAttractive;
                
                if (effectAppliedToPlayer)
                {
                    /*
                     * Apply effect if:
                     * - Player field is not active
                     * - Player field is active but with the same polarity
                     */
                    float fieldSign = fieldAttractive ? 1 : -1;
                    Vector2 fieldDirection_Raw = (Vector2)transform.position - _targetRb.position;
                    float distance = Mathf.Sqrt(Vector2.SqrMagnitude(fieldDirection_Raw));
                    float fieldMagnitude = fieldStrengthCurve.Evaluate(distance / _fieldExtent) * baseFieldIntensity;
                    Vector2 fieldForce = new Vector2((fieldMagnitude * fieldSign / distance) * fieldDirection_Raw.x, 0);
                    
                    _targetPlayerMovement.SetForce(fieldForce);
                }
                else
                {
                    _targetPlayerMovement.SetForce(Vector2.zero);
                }
            }
        }

        private void Update()
        {
            bool effectAppliedToPlayer = false;
            bool updateShaderProps = false;
            if (_targetPm)
            {
                if (_targetPm.Field)
                {
                    Polarity playerPolarity = _targetPm.Field.playerPolarity > 0 ? Polarity.Red : Polarity.Blue;
                    //Apply repulsive/attractive field depending on player polarity and whether their magnetic field is active or not
                    bool playerFieldActive = _targetPm.Field.GetCurrentIntensity() > 0.1;
                    bool fieldAttractive = playerPolarity != polarity;
                    effectAppliedToPlayer = !playerFieldActive || !fieldAttractive;
                    updateShaderProps = true;
                }
            }
            
            if (updateShaderProps)
            {
                Polarity playerPolarity = _targetPm.Field.playerPolarity > 0 ? Polarity.Red : Polarity.Blue;
                if (playerPolarity != polarity)
                {
                    _material.SetFloat("_Mask_Enable", 1);
                    _material.SetVector("_Player_Mask_Position", (Vector2)_targetPm.transform.position);
                    _material.SetFloat("_Magnetic_Field_Intensity", _targetPm.Field.GetCurrentIntensity() / 3);
                }
                else
                {
                    _material.SetFloat("_Mask_Enable", 0);
                }
            }
            
            if (_applyForceField)
            {
                if (!(effectAppliedToPlayer && _prevEffectAppliedToPlayer))
                {
                    OnEffectApplicationChange(effectAppliedToPlayer);
                    _prevEffectAppliedToPlayer = effectAppliedToPlayer;
                }
            }
        }

        public void UpdateShaderProperties()
        {
            _material.SetColor("_Color", polarity == Polarity.Blue ? blueColor : redColor);
            _material.SetFloat("_UV_Scale", transform.localScale.x);
        }

        private void OnDisable()
        {
            if (_targetPlayerMovement)
            {
                _targetPlayerMovement.SetForce(Vector2.zero);
            }
        }
    }
}