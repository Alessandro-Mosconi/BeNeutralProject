using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivator : MonoBehaviour
{
    private MagneticField _playerMagneticField;
    public bool platformCanMove = false;

    [SerializeField] private int activatorPolarity = 1;
    
    void Start()
    {
        var fieldRender = gameObject.GetComponent<Renderer>();
        if (activatorPolarity > 0)
        {
            fieldRender.material.SetColor("_Color", Color.red);
        }
        else
        {
            fieldRender.material.SetColor("_Color", Color.blue);
        }
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
        var tempPlayerMagneticField = other.collider.GetComponent<MagneticField>();
        if (activatorPolarity == tempPlayerMagneticField.playerPolarity)
        {
            _playerMagneticField = tempPlayerMagneticField;
        }
        else
        {
            _playerMagneticField = null;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {

        _playerMagneticField = null;
    }
    
}
