using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivator : MonoBehaviour
{
    private MagneticField _playerMagneticField;
    public bool platformCanMove = false;
    [ColorUsage(showAlpha: true, hdr: true)]
    public Color redColor = Color.red;
    [ColorUsage(showAlpha: true, hdr: true)]
    public Color blueColor = Color.blue;

    [SerializeField] private int activatorPolarity = 1;

    private Material _material;
    
    void Start()
    {
        _material = gameObject.GetComponent<Renderer>().material;
        if (activatorPolarity > 0)
        {
            _material.SetColor("_TintColor", redColor);
            _material.SetColor("_Color", redColor);
        }
        else
        {
            _material.SetColor("_TintColor", blueColor);
            _material.SetColor("_Color", blueColor);
        }
        
        _material.SetFloat("_EffectIntensity", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerMagneticField != null)
        {
            platformCanMove = _playerMagneticField.isActive;
            _material.SetFloat("_EffectIntensity", 1);
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
