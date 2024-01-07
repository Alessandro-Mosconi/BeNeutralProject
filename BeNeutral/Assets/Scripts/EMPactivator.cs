using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPactivator : MonoBehaviour
{

    [SerializeField] private EMPwall emp;
    // Start is called before the first frame update
    void Start()
    {
        
        emp.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        emp.gameObject.SetActive(true);
        emp.isActive = true;
    }
}
