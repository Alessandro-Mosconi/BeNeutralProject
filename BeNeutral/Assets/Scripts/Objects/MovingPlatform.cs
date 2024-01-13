using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed;
    [SerializeField] private float _checkDistance = 0.05f;
    
    [SerializeField] private PlatformActivator activator;
    [SerializeField] private Transform _targetWaypoint;

    [SerializeField] private int _currentWaypointIndex = 0;
    [SerializeField] private bool moveWithoutActivator = false;

    [ColorUsage(showAlpha: true, hdr: true)]
    public Color electricityColor = Color.blue;
    
    [SerializeField] private bool isCrush = false;
    private bool touchGround = false;
    private Renderer _renderer;
    private Material _material;
    
    void Start()
    {
        _targetWaypoint = _waypoints[0];
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        
        _material.SetColor("_Color", electricityColor);
        _material.SetFloat("_EffectIntensity", 0);
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
            _material.SetFloat("_EffectIntensity", 1);
        }
        else
        {
            _material.SetFloat("_EffectIntensity", 0);
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
