using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.Weapons
{
    public class HammerComponent : MonoBehaviour
    {
        public Vector3 rotationPivot = new Vector3(1, 0, 0);
        public float speedDown = 1;
        public float speedUp = 1;
        public float waitTime = 0.5f;
        
        [HideInInspector]
        public System.Action OnHammeringStart = () => { };
        [HideInInspector]
        public System.Action OnHammeringEnd = () => { };

        private bool _isWaiting = false;
        private bool _isPlaying = false;
        private float _cumulatedWaitTime = 0;
        private float _startingAngle = -999;
        private float _currentSpeed;

        public void ResetHammer()
        {
            _isWaiting = false;
            _isPlaying = false;
            _cumulatedWaitTime = 0;
            _startingAngle = transform.localRotation.eulerAngles.z;
            enabled = false; //Disable at the beginning!
        }

        private void Update()
        {
            if (!_isWaiting)
            {
                if (!_isPlaying)
                {
                    _isPlaying = true;
                    _currentSpeed = speedDown;
                    OnHammeringStart();
                }
                transform.RotateAround(transform.TransformPoint(rotationPivot), new Vector3(0, 0, 1), -Mathf.Sign(transform.lossyScale.x) * _currentSpeed * Mathf.Rad2Deg * Time.deltaTime);
                if (transform.localRotation.eulerAngles.z <= 270)
                {
                    transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 271);
                    _isWaiting = true;
                } else if (transform.localRotation.eulerAngles.z >= _startingAngle)
                {
                    enabled = false; //Disable animation!
                    transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, _startingAngle);
                    _isPlaying = false;
                    OnHammeringEnd();
                }
            }
            else
            {
                _cumulatedWaitTime += Time.deltaTime;
                if (_cumulatedWaitTime >= waitTime)
                {
                    _isWaiting = false;
                    //Bring the hammer back up
                    _currentSpeed = -speedUp;
                    _cumulatedWaitTime = 0;
                }
            }
        }
    }
}