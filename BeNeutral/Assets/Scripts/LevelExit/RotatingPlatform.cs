using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{

    [SerializeField] private Transform rotationCenter;

    [SerializeField] private float angularSpeed;

    [SerializeField]private float rotationRadius;
    [SerializeField] private float angle;

    private float posX, posY;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
        posY = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector2(posX, posY);
        angle = angle + Time.deltaTime * angularSpeed;

        if (angle >= 360)
        {
            angle = 0;
        }

    }
}
