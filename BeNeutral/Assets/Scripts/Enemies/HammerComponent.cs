using System;
using UnityEngine;
using UnityEngine.Serialization;

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
    private float _startingAngle;
    private float _currentSpeed;
    
    private void Start()
    {
        _startingAngle = transform.eulerAngles.z;
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
            transform.RotateAround(transform.TransformPoint(rotationPivot), new Vector3(0, 0, 1), _currentSpeed * Mathf.Rad2Deg * Time.deltaTime);
            if (transform.eulerAngles.z >= 90)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
                _isWaiting = true;
            } else if (transform.eulerAngles.z <= _startingAngle)
            {
                enabled = false; //Disable animation!
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