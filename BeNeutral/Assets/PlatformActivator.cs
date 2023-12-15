using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivator : MonoBehaviour
{
    public MagneticField _playerMagneticField;
    public bool platformCanMove = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerMagneticField != null)
        {
            platformCanMove = _playerMagneticField.isActive;
        }
        else
        {
            platformCanMove = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        _playerMagneticField = other.collider.GetComponent<MagneticField>();
    }
    private void OnCollisionExit2D(Collision2D other)
    {

        _playerMagneticField = null;
    }
    
}
