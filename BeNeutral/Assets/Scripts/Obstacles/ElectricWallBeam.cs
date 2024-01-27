using System;
using UnityEngine;

namespace Obstacles
{
    public class ElectricWallBeam : MonoBehaviour
    {
        [HideInInspector] public PlayerManager target;
        [HideInInspector] public bool bluePolarity = true;
        [ColorUsage(showAlpha:true, hdr:true)]
        [HideInInspector] public Color blueColor, redColor;

        private Material _wallMaterial;
        private Rigidbody2D _player1RB, _player2RB;
        private MagneticField _player1MF, _player2MF;
        private Vector2 _targetPosition;
        private float _tgtFieldIntensity;
        private bool _tgtRepelsField;
        private Vector2 _triggerEnterPosition;
        private bool _colorUpdated = false;

        private void Start()
        {
            _wallMaterial = GetComponent<SpriteRenderer>().material;
            
            UpdateColor();
            UpdateTarget();
        }

        private void Update()
        {
            //Update current target and shader params
            UpdateColor();
            UpdateTarget();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_tgtRepelsField || _tgtFieldIntensity < 0.5f)
            {
                target.DamagePlayer(target.HazardDamage);
                target.transform.position =
                    new Vector3(
                        _targetPosition.x > transform.position.x
                            ? (_targetPosition.x - 0.2f)
                            : (_targetPosition.x + 0.2f), _targetPosition.y, 0);
                target.StopMovementUntilKeyup();
            }

            _triggerEnterPosition = _targetPosition;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_tgtRepelsField || _tgtFieldIntensity < 0.5f)
            {
                float offsetDir = Mathf.Sign((_triggerEnterPosition - _targetPosition).x);
                Vector2 newPos = _triggerEnterPosition + new Vector2(offsetDir * 0.2f, 0);
                target.DamagePlayer(target.HazardDamage);
                target.transform.position = new Vector3(newPos.x, newPos.y, 0);
                target.StopMovementUntilKeyup();
            }
        }

        private void UpdateColor()
        {
            if (blueColor is not { r: 0, g: 0, b: 0 })
            {
                Color newColor = blueColor;
                if (!bluePolarity)
                {
                    newColor = redColor;
                }
            
                _wallMaterial.SetColor("_Base_Color", newColor);
                _colorUpdated = true;
            }
        }

        private void UpdateTarget()
        {
            if (!target)
            {
                return;
            }
            
            _targetPosition = target.transform.position;
            _tgtFieldIntensity = target.Field.GetCurrentIntensity();
            _tgtRepelsField = (target.Field.playerPolarity > 0 && bluePolarity) || (target.Field.playerPolarity < 0 && !bluePolarity);
            
            _wallMaterial.SetFloat("_Magnetic_Field_Intensity", _tgtFieldIntensity);
            _wallMaterial.SetVector("_Player_Mask_Position", _targetPosition);
            _wallMaterial.SetFloat("_Mask_Enable", _tgtRepelsField ? 1 : 0);
        }
    }
}