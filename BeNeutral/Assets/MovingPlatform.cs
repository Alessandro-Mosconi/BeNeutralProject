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
    [SerializeField] private bool moveWithoutActivator = false;
    
    [SerializeField] private bool isCrush = false;
    private bool touchGround = false;
    
    void Start()
    {
        _targetWaypoint = _waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (isCrush && !touchGround)
        {
            _targetWaypoint = null;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            return;
        }
        
        if (moveWithoutActivator || (activator != null && activator.platformCanMove && _targetWaypoint!=null))
        {
            Vector2 vectorMove =Vector2.MoveTowards(
                transform.position,
                _targetWaypoint.position,
                _speed * Time.deltaTime);
            
            transform.position = new Vector3(vectorMove.x, vectorMove.y, 70);

            if (Vector2.Distance(transform.position, _targetWaypoint.position) < _checkDistance)
            {
                _targetWaypoint = GetNextWaypoint();
                touchGround = false;
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
        var _playerMovement = other.collider.GetComponent<PlayerMovement>();
        if (_playerMovement != null)
        {
            _playerMovement.setParent(transform);
        }
        var _boxForces = other.collider.GetComponent<BoxForces>();
        if (_boxForces != null)
        {
            _boxForces.setParent(transform);
        }
        if (isCrush && other.gameObject.layer == LayerMask.NameToLayer("Terrain") ||
            other.gameObject.layer == LayerMask.NameToLayer("External-objects") ||
            other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            touchGround = true;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            _targetWaypoint = _waypoints[0];
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        var _playerMovement = other.collider.GetComponent<PlayerMovement>();
        if (_playerMovement != null)
        {
            _playerMovement.resetParent();
        }
        var _boxForces = other.collider.GetComponent<BoxForces>();
        if (_boxForces != null)
        {
            _boxForces.resetParent();
        }
    }
}
