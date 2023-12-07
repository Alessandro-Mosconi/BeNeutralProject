using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public float speed = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print(other.gameObject.layer);
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            print("eccomi");
            gameObject.SetActive(false);
        }
    }
}