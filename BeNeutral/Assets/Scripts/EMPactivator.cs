using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPactivator : MonoBehaviour
{

    [SerializeField] private EMPwall emp;
    [SerializeField] private bool disactivate = false;
    // Start is called before the first frame update
    void Start()
    {
        
        emp.gameObject.SetActive(disactivate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        emp.gameObject.SetActive(!disactivate);
        emp.isActive = !disactivate;
    }
}
