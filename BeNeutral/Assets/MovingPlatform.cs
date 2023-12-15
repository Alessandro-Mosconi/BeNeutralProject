using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed;
    [SerializeField] private float _checkDistance = 0.05f;
    
    [SerializeField] private PlatformActivator activator;
    [SerializeField] private Transform _targetWaypoint;

    [SerializeField] private int _currentWaypointIndex = 0;
    
    void Start()
    {
        _targetWaypoint = _waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (activator != null && activator.platformCanMove)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                _targetWaypoint.position,
                _speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _targetWaypoint.position) < _checkDistance)
            {
                _targetWaypoint = GetNextWaypoint();
            }
        }
        
    }
    
    private Transform GetNextWaypoint()
    {
        _currentWaypointIndex++;
        {
            if (_currentWaypointIndex >= _waypoints.Length)
                _currentWaypointIndex = 0;
        }
        return _waypoints[_currentWaypointIndex];
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print("sono dentro");
        var _playerMovement = other.collider.GetComponent<PlayerMovement>();
        if (_playerMovement != null)
        {
            _playerMovement.setParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        var _playerMovement = other.collider.GetComponent<PlayerMovement>();
        if (_playerMovement != null)
        {
            _playerMovement.resetParent();
        }
    }
}
